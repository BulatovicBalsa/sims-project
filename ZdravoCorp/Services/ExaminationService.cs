using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Models;
using ZdravoCorp.Models.Doctors;
using ZdravoCorp.Repositories;

namespace ZdravoCorp.Services
{
    public class ExaminationService: IService<Examination>
    {
        private readonly ExaminationRepository _examinationRepository;

        public ExaminationService(ExaminationRepository ExaminationRepository)
        {
            _examinationRepository = ExaminationRepository;
        }

        public IEnumerable<Examination> GetAll()
        {
            return _examinationRepository.GetAll();
        }

        public Examination? GetById(int id)
        {
            return _examinationRepository.GetById(id);
        }

        public void Add(Examination item)
        {
            if (!IsFree(item.Doctor, item.Start)) throw new Exception("Doctor is busy");
            // TODO: check if patient is free
            _examinationRepository.Add(item);
        }

        public void Update(Examination item)
        {
            _examinationRepository.Update(item);
        }

        public void Delete(Examination item)
        {
            _examinationRepository.Delete(item);
        }

        public List<Examination> GetExaminationsForDate(Doctor doctor, DateTime requestedDate)
        {
            return _getAll(doctor).Where(examination => examination.Start.Date == requestedDate.Date).ToList();
        }

        public List<Examination> GetExaminationsForNextThreeDays(Doctor doctor) 
        {
            DateTime today = DateTime.Today;

            return _getAll(doctor).Where(examination => examination.Start.Date <= today && examination.Start.Date.AddDays(3) > today).ToList(); // 3 days must be greater then today (not equall) because it would be 4th day from today
        }

        private List<Examination> _getAll(Doctor doctor)
        {
            List<Examination> doctorExaminations = _examinationRepository.GetAll()
            .Where(appointment => appointment.Doctor.Equals(doctor))
            .ToList();
            return doctorExaminations;
        }

        public bool IsFree(Doctor doctor, DateTime start)
        {
            var allExaminations = _getAll(doctor);
            bool isAvailable = !allExaminations.Any(examination => examination.DoesInterfereWith(start));

            return isAvailable;
        }

    }
}
