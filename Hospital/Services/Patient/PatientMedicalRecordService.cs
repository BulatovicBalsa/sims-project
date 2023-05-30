using Hospital.Models.Examination;
using Hospital.Models.Patient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Repositories.Examination;

namespace Hospital.Services
{
    public class PatientMedicalRecordService
    {
        private ExaminationRepository _examinationRepository;

        public PatientMedicalRecordService()
        {
            _examinationRepository = ExaminationRepository.Instance;
        }
        public List<Examination> GetPatientExaminations(Patient patient)
        {
            return _examinationRepository.GetAll(patient);
        }
       
        
    }
}
