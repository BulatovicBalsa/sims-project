using Hospital.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Repositories.Patient
{
    using Hospital.Models.Patient;
    public class PatientRepository
    {
        private const string FilePath = "../../../Data/patient.csv";

        public List<Patient> GetAll()
        {
            return Serializer<Patient>.FromCSV(FilePath);
        }

        public Patient? GetById(string id)
        {
            return GetAll().Find(patient => patient.Id == id);
        }

        public void Add(Patient patient)
        {
            var allPatient = GetAll();
            allPatient.Add(patient);
            Serializer<Patient>.ToCSV(allPatient,FilePath);
        }

        public void Update(Patient patient)
        {
            var allPatient = GetAll();
            var indexToUpdate = allPatient.FindIndex(e => e.Id == patient.Id);
            if (indexToUpdate == -1) throw new KeyNotFoundException();
            allPatient[indexToUpdate] = patient;
            Serializer<Patient>.ToCSV(allPatient,FilePath);
        }

        public void Delete(Patient patient)
        {
            var allPatient = GetAll();
            var indexToDelete = allPatient.FindIndex(e => e.Id == patient.Id);
            if(indexToDelete == -1) throw new KeyNotFoundException();
            allPatient.RemoveAt(indexToDelete);
            Serializer<Patient>.ToCSV(allPatient,FilePath);
        }
    }
}
