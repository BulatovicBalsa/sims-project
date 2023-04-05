using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Models;
using ZdravoCorp.Models.Doctors;
using ZdravoCorp.Models.Patients;
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

        public void Add(Examination examination)
        {
            if (!IsFree(examination.Doctor, examination.Start)) throw new Exception("Doctor is busy");
            if (!IsFree(examination.Patient, examination.Start)) throw new Exception("Patient is busy");
            _examinationRepository.Add(examination);
        }

        public void Update(Examination examination, bool isPatient)
        {
            if (!IsFree(examination.Doctor, examination.Start)) throw new Exception("Doctor is busy");
            if (!IsFree(examination.Patient, examination.Start)) throw new Exception("Patient is busy");
            if (examination.Start < DateTime.Now.AddDays(Patient.MINIMUM_DAYS_TO_CHANGE_OR_DELETE_APPOINTMENT) && isPatient)
                throw new Exception("It is not possible to schedule an appointment less than 24 hours in advance.");

            _examinationRepository.Update(examination);
        }

        public void Delete(Examination examination,bool isPatient)
        {
            _examinationRepository.Delete(examination);
        }

        public List<Examination> GetExaminationsForDate(Doctor doctor, DateTime requestedDate)
        {
            return _getAll(doctor).Where(examination => examination.Start.Date == requestedDate.Date).ToList();
        }
        public List<Examination> GetExaminationsForDate(Patient patient, DateTime requestedDate)
        {
            return _getAll(patient).Where(examination => examination.Start.Date == requestedDate.Date).ToList();
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

        private List<Examination> _getAll(Patient patient)
        {
            List<Examination> patientExaminations = _examinationRepository.GetAll()
            .Where(appointment => appointment.Patient.Equals(patient))
            .ToList();
            return patientExaminations;
        }

        public bool IsFree(Doctor doctor, DateTime start)
        {
            var allExaminations = _getAll(doctor);
            bool isAvailable = !allExaminations.Any(examination => examination.DoesInterfereWith(start));

            return isAvailable;
        }

        public bool IsFree(Patient patient, DateTime start)
        {
            var allExaminations = _getAll(patient);
            bool isAvailable = !allExaminations.Any(examination => examination.DoesInterfereWith(start));

            return isAvailable;
        }

    }
}
