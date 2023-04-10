using Hospital.Models.Examination;
using Hospital.Models.Manager;
using Hospital.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Models.Doctor;

namespace Hospital.Repositories.Examinaton
{
    using Hospital.Models.Patient;
    using Hospital.Serialization.Mappers;

    public class ExaminationRepository
    {
        private const string FilePath = "../../../Data/examination.csv";
        private readonly ExaminationChangesTracker _examinationChangesTracker;

        public ExaminationRepository(ExaminationChangesTracker examinationChangesTracker)
        { 
            _examinationChangesTracker = examinationChangesTracker;
        }


        public List<Examination> GetAll()
        {
            return Serializer<Examination, ExaminationMapper>.FromCSV(FilePath);
        }

        public Examination? GetById(string id)
        {
            return GetAll().Find(examination => examination.Id == id);
        }

        public void Add(Examination examination, bool isMadeByPatient)
        {
            var allExamination = GetAll();

            if (!IsFree(examination.Doctor, examination.Start)) throw new Exception("Doctor is busy");
            if (!IsFree(examination.Patient, examination.Start)) throw new Exception("Patient is busy");
            if (isMadeByPatient)
            {
                PatientExaminationLog log = new(examination.Patient, true);
                _examinationChangesTracker.Add(log);
            }

            allExamination.Add(examination);

            Serializer<Examination, ExaminationMapper>.ToCSV(allExamination, FilePath);
        }

        public void Update(Examination examination,bool isMadeByPatient)
        {
            var allExamination = GetAll();

            var indexToUpdate = allExamination.FindIndex(e => e.Id == examination.Id);
            if (indexToUpdate == -1) throw new KeyNotFoundException();

            if (!IsFree(examination.Doctor, examination.Start,examination.Id)) throw new Exception("Doctor is busy");
            if (!IsFree(examination.Patient, examination.Start, examination.Id)) throw new Exception("Patient is busy");
            if (isMadeByPatient)
            {
                ValidateExaminationTiming(examination.Start);
                ValidateMaxChangesOrDeletesLast30Days(examination.Patient);
                ValidateMaxAllowedExaminationsLast30Days(examination.Patient);
                PatientExaminationLog log = new(examination.Patient, false);
                _examinationChangesTracker.Add(log);
            }

            allExamination[indexToUpdate] = examination;

            Serializer<Examination, ExaminationMapper>.ToCSV(allExamination, FilePath);
        }

        public void Delete(Examination examination, bool isMadeByPatient)
        {
            var allExamination = GetAll();

            var indexToDelete = allExamination.FindIndex(e => e.Id == examination.Id);
            if (indexToDelete == -1) throw new KeyNotFoundException();

            if (!IsFree(examination.Doctor, examination.Start)) throw new Exception("Doctor is busy");
            if (!IsFree(examination.Patient, examination.Start)) throw new Exception("Patient is busy");
            if (isMadeByPatient)
            {
                ValidateExaminationTiming(examination.Start);
                ValidateMaxChangesOrDeletesLast30Days(examination.Patient);
                ValidateMaxAllowedExaminationsLast30Days(examination.Patient);
                PatientExaminationLog log = new(examination.Patient, false);
                _examinationChangesTracker.Add(log);
            }

            allExamination.RemoveAt(indexToDelete);

            Serializer<Examination, ExaminationMapper>.ToCSV(allExamination, FilePath);
        }

        private List<Examination> _getAll(Doctor doctor)
        {
            List<Examination> doctorExaminations = GetAll()
                .Where(appointment => appointment.Doctor.Equals(doctor))
                .ToList();
            return doctorExaminations;
        }

        private List<Examination> _getAll(Patient patient)
        {
            List<Examination> patientExaminations = GetAll()
                .Where(appointment => appointment.Patient.Equals(patient))
                .ToList();
            return patientExaminations;
        }

        public List<Examination> GetExaminationsForDate(Doctor doctor, DateTime requestedDate)
        {
            return _getAll(doctor).Where(examination => examination.Start.Date == requestedDate.Date).ToList();
        }
        public List<Examination> GetExaminationsForDate(Patient patient, DateTime requestedDate)
        {
            return _getAll(patient).Where(examination => examination.Start.Date == requestedDate.Date).ToList();
        }

        public bool IsFree(Doctor doctor, DateTime start, string examinationId = null)
        {
            var allExaminations = _getAll(doctor);
            bool isAvailable = !allExaminations.Any(examination => examination.Id != examinationId && examination.DoesInterfereWith(start));

            return isAvailable;
        }

        public bool IsFree(Patient patient, DateTime start, string examinationId = null)
        {
            var allExaminations = _getAll(patient);
            bool isAvailable = !allExaminations.Any(examination => examination.Id != examinationId && examination.DoesInterfereWith(start));

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

        public void DeleteAll()
        {
            List<Examination> emptyList = new List<Examination>();
            Serializer<Examination, ExaminationMapper>.ToCSV(emptyList, FilePath);
        }
    }
}
