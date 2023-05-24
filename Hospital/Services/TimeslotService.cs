using System;
using System.Collections.Generic;
using System.Linq;
using Hospital.Models.Doctor;
using Hospital.Repositories.Examinaton;

namespace Hospital.Services;

public class TimeslotService
{
    private readonly ExaminationRepository _examinationRepository;
    private readonly ExaminationService _examinationService;

    public TimeslotService()
    {
        _examinationRepository = new ExaminationRepository();
        _examinationService = new ExaminationService();
    }

    public DateTime GetEarliestFreeTimeslot(Doctor doctor)
    {
        var upcomingDoctorExaminations = _examinationService.GetUpcomingExaminations(doctor);

        var earliestTimeslot = GetClosestTimeslot(DateTime.Now);
        while (upcomingDoctorExaminations.Any(examination => AreDatesEqual(examination.Start, earliestTimeslot)))
            earliestTimeslot = earliestTimeslot.AddMinutes(15);

        return earliestTimeslot;
    }

    public List<TimeOnly> GetFreeTimeslotsForDate(Doctor doctor, DateTime date)
    {
        var examinationsForDate = _examinationService.GetExaminationsForDate(doctor, date);
        var freeTimeslots = new List<TimeOnly>();
        var startedIterating = false;

        for (var start = TimeOnly.MinValue; start != TimeOnly.MinValue || !startedIterating; start = start.AddMinutes(15))
        {
            startedIterating = true;
            if (!examinationsForDate.Any(examination =>
                    AreTimesEqual(new TimeOnly(examination.Start.Minute, examination.Start.Second), start)))
            {
                freeTimeslots.Add(start);
            }
        }

        return freeTimeslots;
    }

    private DateTime GetClosestTimeslot(DateTime time)
    {
        var minuteStartTime = time.AddSeconds(-time.Second);
        var validExaminationTimeslot = minuteStartTime.AddMinutes(15 - minuteStartTime.Minute % 15);

        return validExaminationTimeslot;
    }

    public static bool AreDatesEqual(DateTime dateTime1, DateTime dateTime2)
    {
        return dateTime1.Date == dateTime2.Date &&
               dateTime1.Hour == dateTime2.Hour &&
               dateTime1.Minute == dateTime2.Minute &&
               dateTime1.Second == dateTime2.Second;
    }

    public static bool AreTimesEqual(TimeOnly time1, TimeOnly time2)
    {
        return time1.Minute == time2.Minute &&
               time1.Second == time2.Second;
    }

    public bool IsIn2Hours(DateTime timeslot)
    {
        var closestTimeslot = GetClosestTimeslot(DateTime.Now);
        var in2Hours = closestTimeslot.AddHours(2);

        return timeslot < in2Hours || AreDatesEqual(timeslot, in2Hours);
    }

    public SortedDictionary<DateTime, Doctor> GetEarliestFreeTimeslotDoctors(List<Doctor> doctors)
    {
        SortedDictionary<DateTime, Doctor> result = new();

        doctors.ForEach(doctor => result.Add(GetEarliestFreeTimeslot(doctor), doctor));

        return result;
    }
}
