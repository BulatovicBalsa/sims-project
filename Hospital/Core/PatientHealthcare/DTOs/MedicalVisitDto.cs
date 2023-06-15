using Hospital.Core.PatientHealthcare.Models;

namespace Hospital.Core.PatientHealthcare.DTOs;

public class MedicalVisitDto
{
    public MedicalVisitDto(Patient patient, HospitalTreatmentReferral referral)
    {
        Patient = patient;
        Referral = referral;
    }

    public Patient Patient { get; set; }
    public HospitalTreatmentReferral Referral { get; set; }
}