using Hospital.Models.Examination;
using Hospital.Models.Patient;
using Hospital.Repositories.Examinaton;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Services
{
    public class PatientMedicalRecordService
    {
        private ExaminationRepository _examinationRepository;

        public PatientMedicalRecordService()
        {
            _examinationRepository = new ExaminationRepository();
        }
        public List<Examination> GetPatientExaminations(Patient patient)
        {
            return _examinationRepository.GetAll(patient);
        }
       
        
    }
}
