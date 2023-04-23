using Hospital.Models.Doctor;
using Hospital.Models.Examination;
using Hospital.Models.Patient;
using Hospital.Repositories.Doctor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PatientModel = Hospital.Models.Patient.Patient;

namespace Hospital.Services.Patient
{
    public class ExaminationRecommenderService
    {
        private DoctorRepository _doctorRepository;
        private PatientModel _patient;
     
        public ExaminationRecommenderService() { }
        public List<Examination> FindAvailableExaminations(Doctor doctor, DateTime startTime, DateTime endTime, DateTime date, string priority)
        {
            //generate logic that find all possible appointments
            

            return null;
        }

    }
}
