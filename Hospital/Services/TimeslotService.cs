﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Models.Doctor;
using Hospital.Repositories.Doctor;
using Hospital.Repositories.Examinaton;

namespace Hospital.Services
{
    public class TimeslotService
    {
        private readonly DoctorRepository _doctorRepository;
        private readonly ExaminationRepository _examinationRepository;

        public TimeslotService()
        {
            _doctorRepository = new DoctorRepository();
            _examinationRepository = new ExaminationRepository();
        }

        public DateTime? GetEarliestFreeTimeslotIn2Hours(Doctor doctor)
        {
            var upcomingDoctorExaminations =  _examinationRepository.GetAll(doctor).Where(examination => examination.Start > DateTime.Now).ToList();
            var lowerTimeBound = NormalizeTime(DateTime.Now);
            var upperTimeBound = lowerTimeBound.AddHours(2);

            for (var time = lowerTimeBound; time <= upperTimeBound; time = time.AddMinutes(15))
            {
                if (upcomingDoctorExaminations.Any(examination => CompareDates(examination.Start, time)))
                {
                    continue;
                }

                return time;
            }

            return null;
        }

        public DateTime GetEarliestFreeTimeslot(Doctor doctor)
        {
            var upcomingDoctorExaminations = _examinationRepository.GetAll(doctor).Where(examination => examination.Start > DateTime.Now).ToList();
            var lowerTimeBound = NormalizeTime(DateTime.Now);

            for (var time = lowerTimeBound;; time = time.AddMinutes(15))
            {
                if (!upcomingDoctorExaminations.Any(examination => CompareDates(examination.Start, time)))
                {
                    return time;
                }
            }
        }

        private DateTime NormalizeTime(DateTime time)
        {
            var validExaminationTimestamp = time.AddSeconds(-time.Second).AddMilliseconds(-time.Millisecond);
            return validExaminationTimestamp.Minute % 15 == 0 ? validExaminationTimestamp : validExaminationTimestamp.AddMinutes(15 - validExaminationTimestamp.Minute % 15);
        }

        private bool CompareDates(DateTime dateTime1, DateTime dateTime2)
        {
            return dateTime1.Date == dateTime2.Date &&
                   dateTime1.Hour == dateTime2.Hour &&
                   dateTime1.Minute == dateTime2.Minute &&
                   dateTime1.Second == dateTime2.Second;
        }
    }
}
