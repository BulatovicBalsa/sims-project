using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Models.Patients;
using ZdravoCorp.Repositories;

namespace ZdravoCorp.Services
{
    internal class PatientService : IService<Patient>
    {
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
    }
}
