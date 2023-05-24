using System;
using System.Collections.Generic;
using System.Linq;
using Hospital.Exceptions;
using Hospital.Models.Doctor;
using Hospital.Models.Examination;
using Hospital.Models.Patient;
using Hospital.Repositories.Examinaton;

namespace Hospital.Services;

public class ExaminationService
{
    private readonly ExaminationRepository _examinationRepository;

    public ExaminationService()
    {
        _examinationRepository = new ExaminationRepository();
    }

    public Examination? GetAdmissibleExamination(Patient patient)
    {
        var admissibleExaminations = GetAdmissibleExaminations(patient);

        if (admissibleExaminations.Count == 0) return null;
        if (admissibleExaminations.Count > 1) throw new MultipleExaminationsOneTimeslotException();

        return admissibleExaminations.First();
    }

    private List<Examination> GetAdmissibleExaminations(Patient patient)
    {
        var patientExaminations = _examinationRepository.GetAll(patient);
        var unadmittedExaminations = patientExaminations.Where(examination => examination.Admissioned != true).ToList();
        var admissibleExaminations = unadmittedExaminations.Where(examination =>
            examination.Start < DateTime.Now.AddMinutes(15) && examination.Start > DateTime.Now).ToList();

        return admissibleExaminations;
    }

    public List<Examination> GetFivePostponableExaminations(SortedDictionary<DateTime, Doctor> earliestFreeTimeslotDoctors)
    {
        var postponableExaminations = GetPostponableExaminations(earliestFreeTimeslotDoctors);

        var fivePostponableExaminations = postponableExaminations.Count > 5
            ? postponableExaminations.Take(5).ToList()
            : postponableExaminations;

        return fivePostponableExaminations;
    }

    private List<Examination> GetPostponableExaminations(SortedDictionary<DateTime, Doctor> earliestFreeTimeslotDoctors)
    {
        var postponableExaminations = new List<Examination>();
        foreach (var (_, doctor) in earliestFreeTimeslotDoctors)
        {
            var nonUrgentUpcomingExaminations = GetNonUrgentUpcomingExaminations(doctor);

            postponableExaminations.AddRange(nonUrgentUpcomingExaminations);
        }

        return postponableExaminations;
    }

    private List<Examination> GetNonUrgentUpcomingExaminations(Doctor doctor)
    {
        var doctorExaminations = _examinationRepository.GetAll(doctor);
        var nonUrgentUpcomingExaminations = doctorExaminations
            .Where(examination => examination.Start > DateTime.Now && !examination.Urgent)
            .OrderBy(examination => examination.Start).ToList();

        return nonUrgentUpcomingExaminations;
    }

    public bool IsPatientBusy(Patient patient, DateTime timeslot)
    {
        var patientExaminations = _examinationRepository.GetAll(patient);

        return patientExaminations.Any(examination => TimeslotService.AreDatesEqual(examination.Start, timeslot));
    }

    public List<Examination> GetUpcomingExaminations(Doctor doctor)
    {
        var doctorExaminations = _examinationRepository.GetAll(doctor);
        var upcomingDoctorExaminations =
            doctorExaminations.Where(examination => examination.Start > DateTime.Now).ToList();

        return upcomingDoctorExaminations;
    }

    public List<Patient> GetViewedPatients(Doctor doctor)
    {
        var finishedExaminations = _examinationRepository.GetFinishedExaminations(doctor);
        var viewedPatients = finishedExaminations.Select(examination => examination.Patient).Distinct().ToList();
        return viewedPatients!;
    }

    public List<Examination> GetExaminationsForNextThreeDays(Doctor doctor)
    {
        return _examinationRepository.GetExaminationsForNextThreeDays(doctor);
    }

    public void AddExamination(Examination examination,bool isMadeByPatient)
    {
        _examinationRepository.Add(examination, isMadeByPatient);
    }

    public void UpdateExamination(Examination examination, bool isMadeByPatient)
    {
        _examinationRepository.Update(examination, isMadeByPatient);
    }

    public void DeleteExamination(Examination examination,bool isMadeByPatient)
    {
        _examinationRepository.Delete(examination, isMadeByPatient);
    }

    public List<Examination> GetAllExaminations(Patient patient)
    {
        return _examinationRepository.GetAll(patient);
    }

    public List<Examination> GetExaminationsForDate(Doctor doctor, DateTime selectedDate)
    {
        return _examinationRepository.GetExaminationsForDate(doctor, selectedDate);
    }
}