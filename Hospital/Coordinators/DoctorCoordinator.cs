using Hospital.Models.Doctor;
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

        public DoctorCoordinator(ExaminationRepository examinationRepository, PatientRepository patientRepository)
        {
            _examinationRepository = examinationRepository;
            _patientRepository = patientRepository;
        }

        public List<Patient> GetViewedPatients(Doctor doctor)
        {
            var finishedExaminations = _examinationRepository.GetFinishedExaminations(doctor);
            List<Patient> patientsWihoutFullData = finishedExaminations.Select(examination => examination.Patient).Distinct().ToList();
            var patientsWithFullData = patientsWihoutFullData.Select(patient => _patientRepository.GetById(patient.Id)).Distinct().ToList();
            return patientsWithFullData;
        }
    }
}
