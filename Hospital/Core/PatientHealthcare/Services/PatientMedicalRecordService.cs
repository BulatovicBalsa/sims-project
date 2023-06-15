using System.Collections.Generic;
using Hospital.Core.PatientHealthcare.Models;
using Hospital.Core.PatientHealthcare.Repositories;

namespace Hospital.Core.PatientHealthcare.Services;

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