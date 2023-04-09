using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using Hospital.Serialization;

namespace Hospital.Repositories.Patient
{
    using Hospital.Models.Patient;
    public class PatientRepository
    {
        private const string FilePath = "..\\Data\\patients.csv";

        public List<Patient> GetAll()
        {
            return Serializer<Patient>.FromCSV(FilePath);
        }

        public Patient? GetById(string id)
        {
            return GetAll().FirstOrDefault(patient => patient.Id == id);
        }

        public void Add(Patient patient)
        {
            var allPatients = GetAll();
            allPatients.Add(patient);
            Serializer<Patient>.ToCSV(allPatients, FilePath);
        }

        public void Update(Patient patient)
        {
            var allPatients = GetAll();

            var indexToUpdate = allPatients.FindIndex(patientRecord => patientRecord.Id == patient.Id);
            if (indexToUpdate == -1) 
                throw new KeyNotFoundException($"Patient with id {patient.Id} was not found.");
            allPatients[indexToUpdate] = patient;

            Serializer<Patient>.ToCSV(allPatients, FilePath);
        }

        public void Delete(Patient patient)
        {
            var patientRecords = GetAll();

            if (!patientRecords.Remove(patient))
                throw new KeyNotFoundException($"Patient with id {patient.Id} was not found.");

            Serializer<Patient>.ToCSV(patientRecords, FilePath);
        }
    }
}
