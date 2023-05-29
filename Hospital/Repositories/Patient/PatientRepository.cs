using System;
using System.Collections.Generic;
using System.Linq;
using Hospital.Repositories.Examination;
using Hospital.Serialization;
using Hospital.Serialization.Mappers.Patient;

namespace Hospital.Repositories.Patient;

using Hospital.Models.Patient;
using Hospital.Repositories.Doctor;

public class PatientRepository
{
    private const string FilePath = "../../../Data/patients.csv";
    private readonly ExaminationRepository _examinationRepository =ExaminationRepository.Instance;
    private static PatientRepository? _instance;

    public event Action<Patient>? PatientAdded;
    public event Action<Patient>? PatientUpdated;

    public static PatientRepository Instance => _instance ??= new PatientRepository();

    private PatientRepository() { }

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

        PatientAdded?.Invoke(patient);
    }

    public void Update(Patient patient)
    {
        var allPatients = GetAll();

        var indexToUpdate = allPatients.FindIndex(patientRecord => patientRecord.Id == patient.Id);
        if (indexToUpdate == -1)
            throw new KeyNotFoundException($"Patient with id {patient.Id} was not found.");
        allPatients[indexToUpdate] = patient;

        Serializer<Patient>.ToCSV(allPatients, FilePath, new PatientWriteMapper());

        PatientUpdated?.Invoke(patient);
    }

    public void Delete(Patient patient)
    {
        var patientRecords = GetAll();

        if (!patientRecords.Remove(patient))
            throw new KeyNotFoundException($"Patient with id {patient.Id} was not found.");

        var patientExaminations = _examinationRepository.GetAll(patient);
        patientExaminations.ForEach(examination => _examinationRepository.Delete(examination, false));


        Serializer<Patient>.ToCSV(patientRecords, FilePath, new PatientWriteMapper());
    }

    public static void DeleteAll()
    {
        var emptyPatientList = new List<Patient>();
        Serializer<Patient>.ToCSV(emptyPatientList, FilePath);
    }
}
