using System.Collections.Generic;
using System.Linq;
using Hospital.Core.PatientHealthcare.DTOs;
using Hospital.Core.Workers.Models;

namespace Hospital.Core.PatientHealthcare.Services;

public class HospitalTreatmentService
{
    private readonly ExaminationService _examinationService = new();

    public List<MedicalVisitDto> GetHospitalizedPatients(Doctor doctor)
    {
        var designatedPatients = _examinationService.GetViewedPatients(doctor);

        return (from patient in designatedPatients
                where patient.IsHospitalized()
                select new MedicalVisitDto(patient, patient.GetActiveHospitalTreatmentReferral())).ToList();
    }
}