using System;
using System.Collections.Generic;
using System.Linq;
using Hospital.PatientHealthcare.Models;
using Hospital.Serialization;
using Hospital.Serialization.Mappers.Patient;

namespace Hospital.PatientHealthcare.Repositories;

public class PatientRepository
{
    private const string FilePath = "../../../Data/patients.csv";
    private static PatientRepository? _instance;
    private readonly ExaminationRepository _examinationRepository = ExaminationRepository.Instance;

    private PatientRepository()
    {
    }

    public static PatientRepository Instance => _instance ??= new PatientRepository();

    public event Action<Patient>? PatientAdded;
    public event Action<Patient>? PatientUpdated;

    public List<Patient> GetAll()
    {
        return CsvSerializer<Patient>.FromCSV(FilePath, new PatientReadMapper());
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
        CsvSerializer<Patient>.ToCSV(allPatients, FilePath, new PatientWriteMapper());

        PatientAdded?.Invoke(patient);
    }

    public void Update(Patient patient)
    {
        var allPatients = GetAll();

        var indexToUpdate = allPatients.FindIndex(patientRecord => patientRecord.Id == patient.Id);
        if (indexToUpdate == -1)
            throw new KeyNotFoundException($"Patient with id {patient.Id} was not found.");
        allPatients[indexToUpdate] = patient;

        CsvSerializer<Patient>.ToCSV(allPatients, FilePath, new PatientWriteMapper());

        PatientUpdated?.Invoke(patient);
    }

    public void Delete(Patient patient)
    {
        var patientRecords = GetAll();

        if (!patientRecords.Remove(patient))
            throw new KeyNotFoundException($"Patient with id {patient.Id} was not found.");

        var patientExaminations = _examinationRepository.GetAll(patient);
        patientExaminations.ForEach(examination => _examinationRepository.Delete(examination, false));


        CsvSerializer<Patient>.ToCSV(patientRecords, FilePath, new PatientWriteMapper());
    }

    public static void DeleteAll()
    {
        var emptyPatientList = new List<Patient>();
        CsvSerializer<Patient>.ToCSV(emptyPatientList, FilePath);
    }

    public List<Patient> GetAllAccommodable()
    {
        var allPatients = GetAll();

        return allPatients.Where(patient => patient.HasUnusedHospitalTreatmentReferral() && !patient.IsHospitalized())
            .ToList();
    }

    public void UpdateHospitalTreatmentReferral(Patient patient, HospitalTreatmentReferral referral)
    {
        patient.UpdateHospitalTreatmentReferral(referral);
        Update(patient);
    }

    public List<Patient> GetAllHospitalized()
    {
        var allPatients = GetAll();
        return allPatients.Where(patient => patient.IsHospitalized()).ToList();
    }
}