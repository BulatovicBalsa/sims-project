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

        public void Delete(Patient patient, bool isPatient)
        {
            _patientRepository.Delete(patient);
        }

        public IEnumerable<Patient> GetAll()
        {
            return _patientRepository.GetAll();
        }

        public Patient? GetById(int id)
        {
            return _patientRepository.GetById(id);
        }

        public void Update(Patient patient, bool isPatient)
        {
            _patientRepository.Update(patient); 
        }

        public void Add(Patient patient)
        {
            _patientRepository.Add(patient);
        }

        
    }
}
