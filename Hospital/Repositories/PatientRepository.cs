using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using Hospital.Models.Patient;

namespace Hospital.Repositories
{
    public class PatientRepository
    {
        private const string FilePath = "..\\Data\\patients.csv";

        public List<Patient> GetAll()
        {
            using var reader = new StreamReader(FilePath);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            return csv.GetRecords<Patient>().ToList();
        }

        public Patient? GetById(string id)
        {
            return GetAll().FirstOrDefault(patient => patient.Id == id);
        }

        public void Add(Patient patient)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = File.ReadLines(FilePath).Count() > 1,
            };
            using var stream = File.Open(FilePath, FileMode.Append);
            using var writer = new StreamWriter(stream);
            using var csv = new CsvWriter(writer, config);
            csv.WriteRecord(patient);
        }

        public void Update(Patient updatedPatient)
        {
            var patientRecords = GetAll();
            var patientToUpdate = patientRecords.FirstOrDefault(patient => patient.Id == updatedPatient.Id) ?? throw new Exception($"Patient with id {updatedPatient.Id} was not found.");
            var indexToUpdate = patientRecords.IndexOf(patientToUpdate);
            patientRecords[indexToUpdate] = updatedPatient;

            using var writer = new StreamWriter(FilePath);
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
            csv.WriteRecords(patientRecords);
        }

        public void Delete(Patient patientToDelete)
        {
            var patientRecords = GetAll();
            if (!patientRecords.Remove(patientToDelete))
            {
                throw new Exception($"Patient with id {patientToDelete.Id} was not found.");
            }

            using var writer = new StreamWriter(FilePath);
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
            csv.WriteRecords(patientRecords);
        }
    }
}
