using System;
using System.Collections.Generic;
using System.Linq;
using Hospital.Models.Doctor;
using Hospital.Repositories.Examinaton;

namespace Hospital.Services;

public class TimeslotService
{
    private readonly ExaminationRepository _examinationRepository;

    public TimeslotService()
    {
        _examinationRepository = new ExaminationRepository();
    }

    public DateTime GetEarliestFreeTimeslot(Doctor doctor)
    {
        var doctorExaminations = _examinationRepository.GetAll(doctor);
        var upcomingDoctorExaminations =
            doctorExaminations.Where(examination => examination.Start > DateTime.Now).ToList();

        var earliestTimeslot = GetClosestTimeslot(DateTime.Now);
        while (upcomingDoctorExaminations.Any(examination => AreDatesEqual(examination.Start, earliestTimeslot)))
            earliestTimeslot = earliestTimeslot.AddMinutes(15);

        return earliestTimeslot;
    }

    private DateTime GetClosestTimeslot(DateTime time)
    {
        var minuteStartTime = time.AddSeconds(-time.Second);
        var validExaminationTimeslot = minuteStartTime.AddMinutes(15 - minuteStartTime.Minute % 15);

        return validExaminationTimeslot;
    }

    public bool AreDatesEqual(DateTime dateTime1, DateTime dateTime2)
    {
        return dateTime1.Date == dateTime2.Date &&
               dateTime1.Hour == dateTime2.Hour &&
               dateTime1.Minute == dateTime2.Minute &&
               dateTime1.Second == dateTime2.Second;
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
