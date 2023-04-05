using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Models;
using ZdravoCorp.Models.Doctors;
using ZdravoCorp.Models.Examinations;
using ZdravoCorp.Models.Patients;
using ZdravoCorp.Repositories;

namespace ZdravoCorp.Services
{
    public class ExaminationService: IService<Examination>
    {
        private readonly ExaminationRepository _examinationRepository;
        private readonly ExaminationChangesTracker _examinationChangesTracker;

        public ExaminationService(ExaminationRepository ExaminationRepository, ExaminationChangesTracker ExaminationChangesTracker)
        {
            _examinationRepository = ExaminationRepository;
            _examinationChangesTracker = ExaminationChangesTracker;
        }

        public IEnumerable<Examination> GetAll()
        {
            return _examinationRepository.GetAll();
        }

        public Examination? GetById(int id)
        {
            return _examinationRepository.GetById(id);
        }

        public void Add(Examination examination, bool isPatient)
        {
            if (!IsFree(examination.Doctor, examination.Start)) throw new Exception("Doctor is busy");
            if (!IsFree(examination.Patient, examination.Start)) throw new Exception("Patient is busy");
            if (isPatient)
            {
                PatientExaminationLog log = new(examination.Patient, true);
                _examinationChangesTracker.Add(log);
            }
            _examinationRepository.Add(examination);
        }

        public void Update(Examination examination, bool isPatient)
        {
            if (!IsFree(examination.Doctor, examination.Start)) throw new Exception("Doctor is busy");
            if (!IsFree(examination.Patient, examination.Start)) throw new Exception("Patient is busy");
            if (isPatient)
            {
                ValidateExaminationTiming(examination.Start);
                ValidateMaxChangesOrDeletesLast30Days(examination.Patient);
                ValidateMaxAllowedExaminationsLast30Days(examination.Patient);
                PatientExaminationLog log = new(examination.Patient, false);
                _examinationChangesTracker.Add(log);
            }
            _examinationRepository.Update(examination);
            
        }

        public void Delete(Examination examination,bool isPatient)
        {
            if (isPatient)
            {
                ValidateExaminationTiming(examination.Start);
                ValidateMaxChangesOrDeletesLast30Days(examination.Patient);
                ValidateMaxAllowedExaminationsLast30Days(examination.Patient);
                PatientExaminationLog log = new(examination.Patient, false);
                _examinationChangesTracker.Add(log);
            }
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
        private void ValidateExaminationTiming(DateTime start)
        {
            if (start < DateTime.Now.AddDays(Patient.MINIMUM_DAYS_TO_CHANGE_OR_DELETE_APPOINTMENT))
            {
                throw new InvalidOperationException("It is not possible to schedule an appointment less than 24 hours in advance.");
            }
        }

        private void ValidateMaxChangesOrDeletesLast30Days(Patient patient)
        {
            if (_examinationChangesTracker.GetNumberOfChangeLogsForPatientInLast30Days(patient) > Patient.MAX_CHANGES_OR_DELETES_LAST_30_DAYS)
            {
                throw new InvalidOperationException("Patient made too many changes in last 30 days");
            }
        }

        private void ValidateMaxAllowedExaminationsLast30Days(Patient patient)
        {
            if (_examinationChangesTracker.GetNumberOfCreationLogsForPatientInLast30Days(patient) > Patient.MAX_ALLOWED_APPOINTMENTS_LAST_30_DAYS)
            {
                throw new InvalidOperationException("Patient made too many examinations in last 30 days");
            }
        }

    }
}
