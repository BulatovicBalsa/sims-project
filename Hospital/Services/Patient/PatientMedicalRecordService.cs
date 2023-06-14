using System.Collections.Generic;
using Hospital.Models.Examination;
using Hospital.Models.Patient;
using Hospital.Repositories.Examination;

namespace Hospital.Services;

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