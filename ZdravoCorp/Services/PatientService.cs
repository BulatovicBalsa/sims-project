using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Models;
using ZdravoCorp.Models.Doctors;
using ZdravoCorp.Models.Patients;
using ZdravoCorp.Repositories;

namespace ZdravoCorp.Services
{
    internal class PatientService : IService<Patient>
    {
        private const int MINIMUM_DAYS_TO_CHANGE_OR_DELETE_APPOINTMENT = 1;
        private const int MAX_CHANGES_OR_DELETES_LAST_30_DAYS = 4;
        private const int MAX_ALLOWED_APPOINTMENTS_LAST_30_DAYS = 8;

        private readonly PatientRepository _patientRepository;

        public void Delete(Patient item)
        {
            _patientRepository.Delete(item);
        }

        public IEnumerable<Patient> GetAll()
        {
            return _patientRepository.GetAll();
        }

        public Patient? GetById(int id)
        {
            return _patientRepository.GetById(id);
        }

        public void Update(Patient item)
        {
            _patientRepository.Update(item); 
        }

        public void Add(Patient item)
        {
            _patientRepository.Add(item);
        }

        public void MakeAppointment(Patient patient,Doctor doctor,DateTime start)
        {
            if (patient.IsBlocked) throw new Exception("Patient profile is blocked");

            Examination examination = new(doctor,patient,false,start);

            if (examination.Start < DateTime.Now.AddDays(MINIMUM_DAYS_TO_CHANGE_OR_DELETE_APPOINTMENT))
                throw new Exception("It is not possible to schedule an appointment less than 24 hours in advance.");

        }

        
    }
}
