using System;
using System.Collections.Generic;
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

        public DateTime? GetEarliestFreeTimeslotIn2Hours(Doctor doctor, bool isOperation)
        {
            var doctorExaminations =  _examinationRepository.GetAll(doctor);
            var lowerTimeBound = NormalizeTime(DateTime.Now);
            var upperTimeBound = lowerTimeBound.AddHours(2);

            for (var time = lowerTimeBound; time <= upperTimeBound; time = time.AddMinutes(15))
            {
                if (doctorExaminations.Any(examination => examination.Start == time))
                {
                    continue;
                }

                if (isOperation && doctorExaminations.Any(examination => examination.Start == time.AddMinutes(15)))
                {
                    continue;
                }

                return time;
            }

            return null;
        }

        private DateTime NormalizeTime(DateTime time)
        {
            return time.Minute % 15 == 0 ? time : time.AddMinutes(15 - time.Minute % 15);
        }
    }
}
