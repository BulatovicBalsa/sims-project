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

    public sealed class PatientWriteMapper : ClassMap<Patient>
    {
        public PatientWriteMapper()
        {
            Map(patient => patient.Id).Index(0);
            Map(patient => patient.FirstName).Index(1);
            Map(patient => patient.LastName).Index(2);
            Map(patient => patient.Jmbg).Index(3);
            Map(patient => patient.Profile.Username).Index(4);
            Map(patient => patient.Profile.Password).Index(5);
            Map(patient => patient.MedicalRecord.Height).Index(6);
            Map(patient => patient.MedicalRecord.Weight).Index(7);
            Map(patient => patient.MedicalRecord.Allergies).Convert(row => string.Join("|", row.Value.MedicalRecord.Allergies)).Index(8);
            Map(patient => patient.MedicalRecord.MedicalHistory).Convert(row => string.Join("|", row.Value.MedicalRecord.MedicalHistory)).Index(9);
            Map(patient => patient.IsBlocked).Index(10);

        }
    }

    public sealed class PatientReadMapper : ClassMap<Patient>
    {
        private List<string> SplitColumnValues(string? columnValue)
        {
            return columnValue?.Split("|").ToList() ?? new List<string>();
        }
        public PatientReadMapper()
        {
            Map(patient => patient.Id).Index(0);
            Map(patient => patient.FirstName).Index(1);
            Map(patient => patient.LastName).Index(2);
            Map(patient => patient.Jmbg).Index(3);
            Map(patient => patient.Profile.Username).Index(4);
            Map(patient => patient.Profile.Password).Index(5);
            Map(patient => patient.MedicalRecord.Height).Index(6);
            Map(patient => patient.MedicalRecord.Weight).Index(7);
            Map(patient => patient.MedicalRecord.Allergies).Index(8).Convert(row => SplitColumnValues(row.Row.GetField<string>("Allergies")));
            Map(patient => patient.MedicalRecord.MedicalHistory).Index(9).Convert(row => SplitColumnValues(row.Row.GetField<string>("MedicalHistory")));
            Map(patient => patient.IsBlocked).Index(10);
        }
    }
    public class PatientRepository
    {
        private const string FilePath = "../../../Data/patients.csv";

        public event Action<Patient> PatientAdded;
        public event Action<Patient> PatientUpdated;

        public List<Patient> GetAll()
        {
            return Serializer<Patient>.FromCSV(FilePath, new PatientReadMapper());
        }

        public Patient? GetById(string id)
        {
            return GetAll().FirstOrDefault(patient => patient.Id == id);
        }

        public Patient? GetByUsername(string username)
        {
            return GetAll().FirstOrDefault(patient => patient.Profile.Username == username);
        }

        public void Add(Patient patient)
        {
            var allPatients = GetAll();
            allPatients.Add(patient);
            Serializer<Patient>.ToCSV(allPatients, FilePath, new PatientWriteMapper());

            PatientAdded.Invoke(patient);
        }

        public void Update(Patient patient)
        {
            var allPatients = GetAll();

            var indexToUpdate = allPatients.FindIndex(patientRecord => patientRecord.Id == patient.Id);
            if (indexToUpdate == -1) 
                throw new KeyNotFoundException($"Patient with id {patient.Id} was not found.");
            allPatients[indexToUpdate] = patient;

            Serializer<Patient>.ToCSV(allPatients, FilePath, new PatientWriteMapper());

            PatientUpdated.Invoke(patient);
        }

        public void Delete(Patient patient)
        {
            var patientRecords = GetAll();

            if (!patientRecords.Remove(patient))
                throw new KeyNotFoundException($"Patient with id {patient.Id} was not found.");


            Serializer<Patient>.ToCSV(patientRecords, FilePath, new PatientWriteMapper());
        }

        public static void DeleteAll()
        {
            var emptyPatientList = new List<Patient>();
            Serializer<Patient>.ToCSV(emptyPatientList, FilePath);
        }
    }
}
