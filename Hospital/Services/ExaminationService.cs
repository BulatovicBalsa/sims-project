using System;
using System.Linq;
using Hospital.Models.Examination;
using Hospital.Models.Patient;
using Hospital.Repositories.Examinaton;

namespace Hospital.Services;

public class ExaminationService
{
    private readonly ExaminationRepository _examinationRepository;

    public ExaminationService()
    {
        _examinationRepository = new ExaminationRepository();
    }

    public Examination? GetAdmissibleExamination(Patient patient)
    {
        var validExaminations = _examinationRepository.GetAll(patient)
            .Where(examination => examination.Admissioned != true).ToList();
        var admissibleExaminations = validExaminations.Where(examination =>
            examination.Start < DateTime.Now.AddMinutes(15) && examination.Start > DateTime.Now).ToList();

        if (admissibleExaminations.Count == 0) return null;
        if (admissibleExaminations.Count > 1) throw new Exception();

        return admissibleExaminations[0];
    }
}