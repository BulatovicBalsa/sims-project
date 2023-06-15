using System;
using System.Collections.Generic;
using System.Linq;
using Hospital.Injectors;
using Hospital.PatientHealthcare.Models;
using Hospital.PatientHealthcare.Repositories;
using Hospital.Scheduling.Models;
using Hospital.Serialization;
using Hospital.Workers.Models;
using Hospital.Workers.Repositories;

namespace Hospital.Scheduling.Services;

public class ExaminationRecommenderService
{
    private const int NumberOfSuggestedExaminations = 3;
    private const int MaxNumberOfClosestExaminations = 6;
    private const int TimeIntervalInMinutes = 10;

    private readonly DoctorRepository _doctorRepository;
    private readonly ExaminationRepository _examinationRepository;

    public ExaminationRecommenderService()
    {
        _doctorRepository =
            new DoctorRepository(SerializerInjector.CreateInstance<ISerializer<Doctor>>());
        _examinationRepository = ExaminationRepository.Instance;
    }

    public List<Doctor> GetAllDoctors()
    {
        return _doctorRepository.GetAll();
    }

    public void AddExamination(Examination examination)
    {
        _examinationRepository.Add(examination, true);
    }

    public List<Examination> FindAvailableExaminations(Patient patient,
        ExaminationSearchOptions options)
    {
        var examinations = SearchByBothCriteria(patient, options);

        if (examinations.Count > 0) return examinations;
        if (options.Priority == Priority.Doctor)
        {
            examinations = SearchByDoctorPriority(patient, options);
            if (examinations.Count > 0) return examinations;
            examinations = SearchByTimeRangePriority(patient, options);
            if (examinations.Count > 0) return examinations;
        }
        else
        {
            examinations = SearchByTimeRangePriority(patient, options);
            if (examinations.Count > 0) return examinations;
            examinations = SearchByDoctorPriority(patient, options);
            if (examinations.Count > 0) return examinations;
        }

        return SearchWithoutPriority(patient, options);
    }

    private List<Examination> SearchExaminations(Patient patient, ExaminationSearchOptions options,
        Doctor doctor, Func<DateTime, TimeRange> getSearchRangeForDay)
    {
        var examinations = new List<Examination>();
        var startDate = DateTime.Now.Date.AddDays(1);

        for (var currentDate = startDate; currentDate <= options.LatestDate; currentDate = currentDate.AddDays(1))
        {
            var timeRange = getSearchRangeForDay(currentDate);
            examinations = SearchInTimeRange(doctor, patient, timeRange, examinations);
            if (examinations.Count >= NumberOfSuggestedExaminations) return examinations;
        }

        return examinations;
    }

    private List<Examination> SearchByBothCriteria(Patient patient, ExaminationSearchOptions options)
    {
        return SearchExaminations(patient, options, options.PreferredDoctor,
            currentDate => new TimeRange(currentDate.Add(options.StartTime), currentDate.Add(options.EndTime)));
    }

    private List<Examination> SearchByDoctorPriority(Patient patient, ExaminationSearchOptions options)
    {
        var examinations = new List<Examination>();

        var today = DateTime.Today;
        var futureStartDate = today.AddDays(1).Add(options.EndTime);
        var futureEndDate = futureStartDate.AddHours(4);
        var futureTimeRange = new TimeRange(futureStartDate, futureEndDate);

        var pastEndDate = today.AddDays(1).Add(options.StartTime);
        var pastStartDate = pastEndDate.AddHours(-4);
        var pastTimeRange = new TimeRange(pastStartDate, pastEndDate);

        examinations = SearchExaminations(patient, options, options.PreferredDoctor, _ => futureTimeRange);
        if (examinations.Count >= NumberOfSuggestedExaminations)
            return examinations;

        examinations = SearchExaminations(patient, options, options.PreferredDoctor, _ => pastTimeRange);
        if (examinations.Count >= NumberOfSuggestedExaminations)
            return examinations;

        return examinations;
    }

    private List<Examination> SearchByTimeRangePriority(Patient patient,
        ExaminationSearchOptions options)
    {
        var examinations = new List<Examination>();
        var doctors = GetAllDoctors();
        var startDate = DateTime.Now.Date.AddDays(1);

        foreach (var doctor in doctors)
            for (var currentDate = startDate; currentDate <= options.LatestDate; currentDate = currentDate.AddDays(1))
            {
                examinations = SearchExaminations(patient, options, doctor,
                    currentDate => new TimeRange(currentDate.Add(options.StartTime), currentDate.Add(options.EndTime)));
                if (examinations.Count >= NumberOfSuggestedExaminations) return examinations;
            }

        return examinations;
    }

    private List<Examination> SearchWithoutPriority(Patient patient, ExaminationSearchOptions options)
    {
        var examinations = GetAllPossibleExaminations(patient, options);
        var closestExaminations = GetClosestExaminations(examinations, options);
        return closestExaminations;
    }

    private List<Examination> GetClosestExaminations(List<Examination> examinations, ExaminationSearchOptions options)
    {
        var examinationRankings = examinations
            .Select(examination => new
            {
                Examination = examination,
                Difference = Math.Abs((examination.Start.TimeOfDay - options.StartTime).TotalMinutes),
                IsPreferredDoctor = examination.Doctor == options.PreferredDoctor
            });

        var closestExaminations = examinationRankings
            .OrderBy(item => item.Difference)
            .ThenByDescending(item => item.IsPreferredDoctor)
            .Select(item => item.Examination)
            .Take(NumberOfSuggestedExaminations)
            .ToList();

        return closestExaminations;
    }

    private List<Examination> GetAllPossibleExaminations(Patient patient,
        ExaminationSearchOptions options)
    {
        var examinations = new List<Examination>();
        var doctors = GetAllDoctors();
        SortDoctorsByPreference(doctors, options.PreferredDoctor);

        var currentDate = DateTime.Now.Date.AddDays(1);
        var searchRange = TimeSpan.FromHours(1);

        while (currentDate <= options.LatestDate)
        {
            var beforeRange = new TimeRange(currentDate.Add(options.StartTime).Subtract(searchRange),
                currentDate.Add(options.StartTime));
            var afterRange = new TimeRange(currentDate.Add(options.EndTime),
                currentDate.Add(options.EndTime).Add(searchRange));

            foreach (var doctor in doctors)
            {
                examinations = SearchInTimeRange(doctor, patient, beforeRange, examinations);
                if (examinations.Count >= MaxNumberOfClosestExaminations) return examinations;
                examinations = SearchInTimeRange(doctor, patient, afterRange, examinations);
                if (examinations.Count >= MaxNumberOfClosestExaminations) return examinations;
            }

            currentDate = currentDate.AddDays(1);
        }

        return examinations;
    }

    private bool IsExaminationTimeFree(Doctor doctor, Patient patient,
        DateTime examinationTime)
    {
        return _examinationRepository.IsFree(doctor, examinationTime) &&
               _examinationRepository.IsFree(patient, examinationTime);
    }

    private void AddExaminationIfTimeFree(Doctor doctor, Patient patient,
        DateTime currentTime, List<Examination> examinations)
    {
        if (IsExaminationTimeFree(doctor, patient, currentTime))
        {
            var examination = new Examination(doctor, patient, false, currentTime, null);
            examinations.Add(examination);
        }
    }

    private List<Examination> SearchInTimeRange(Doctor doctor, Patient patient,
        TimeRange timeRange, List<Examination> examinations)
    {
        var currentTime = timeRange.StartTime;

        while (currentTime <= timeRange.EndTime)
        {
            AddExaminationIfTimeFree(doctor, patient, currentTime, examinations);
            if (examinations.Count >= MaxNumberOfClosestExaminations) return examinations;
            currentTime = currentTime.AddMinutes(TimeIntervalInMinutes);
        }

        return examinations;
    }

    private void SortDoctorsByPreference(List<Doctor> doctors, Doctor preferredDoctor)
    {
        doctors.Remove(preferredDoctor);
        doctors.Insert(0, preferredDoctor);
    }
}