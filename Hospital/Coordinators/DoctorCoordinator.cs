using Hospital.Models.Doctor;
using Hospital.Models.Examination;
using Hospital.Models.Patient;
using Hospital.Repositories.Examinaton;
using Hospital.Repositories.Patient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Coordinators
{
    public class DoctorCoordinator
    {
        private readonly ExaminationRepository _examinationRepository;
        private readonly PatientRepository _patientRepository;

        public DoctorCoordinator()
        {
            _examinationRepository = new ExaminationRepository(new ExaminationChangesTracker());
            _patientRepository = new PatientRepository();
        }

        public List<Patient> GetViewedPatients(Doctor doctor)
        {
            var finishedExaminations = _examinationRepository.GetFinishedExaminations(doctor);
            List<Patient> patientsWihoutFullData = finishedExaminations.Select(examination => examination.Patient).Distinct().ToList();
            var patientsWithFullData = patientsWihoutFullData.Select(patient => _patientRepository.GetById(patient.Id)).Distinct().ToList();
            return patientsWithFullData;
        }

        public Patient GetPatient(Examination examination)
        {
            return _patientRepository.GetById(examination.Patient.Id);
        }

        public void UpdatePatient(Patient patient)
        {
            _patientRepository.Update(patient);
        }

        public void UpdateExamination(Examination examination)
        {
            _examinationRepository.Update(examination, false);
        }
    }
}
