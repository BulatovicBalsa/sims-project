using System.Collections.Generic;
using Hospital.PatientHealthcare.Models;
using Hospital.PatientHealthcare.Repositories;

namespace Hospital.PatientHealthcare.Services;

public class PatientMedicalRecordService
{
    private readonly ExaminationRepository _examinationRepository;

    public PatientMedicalRecordService()
    {
        _examinationRepository = ExaminationRepository.Instance;
    }

    public List<Examination> GetPatientExaminations(Patient patient)
    {
        return _examinationRepository.GetAll(patient);
    }
}