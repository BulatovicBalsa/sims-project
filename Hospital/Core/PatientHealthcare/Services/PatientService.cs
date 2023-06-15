using System.Collections.Generic;
using Hospital.Core.PatientHealthcare.Models;
using Hospital.Core.PatientHealthcare.Repositories;

namespace Hospital.Core.PatientHealthcare.Services;

public class PatientService
{
    private readonly PatientRepository _patientRepository;

    public PatientService()
    {
        _patientRepository = PatientRepository.Instance;
    }

    public Patient GetPatient(Examination examination)
    {
        return _patientRepository.GetById(examination.Patient!.Id)!;
    }

    public List<Patient> GetAllPatients()
    {
        return _patientRepository.GetAll();
    }

    public void UpdatePatient(Patient patient)
    {
        _patientRepository.Update(patient);
    }

    public Patient? GetPatientById(string patientId)
    {
        return _patientRepository.GetById(patientId);
    }

    public List<Patient> GetAllAccommodablePatients()
    {
        return _patientRepository.GetAllAccommodable();
    }

    public void UpdateHospitalTreatmentReferral(Patient patient, HospitalTreatmentReferral referral)
    {
        _patientRepository.UpdateHospitalTreatmentReferral(patient, referral);
    }

    public List<Patient> GetAllHospitalizedPatients()
    {
        return _patientRepository.GetAllHospitalized();
    }
}