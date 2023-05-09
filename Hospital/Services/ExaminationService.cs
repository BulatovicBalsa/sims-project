using System;
using System.Collections.Generic;
using System.Linq;
using Hospital.Models.Doctor;
using Hospital.Models.Examination;
using Hospital.Models.Patient;
using Hospital.Repositories.Examinaton;
using static Xceed.Wpf.Toolkit.Calculator;

namespace Hospital.Services;

public class ExaminationService
{
    private readonly ExaminationRepository _examinationRepository;
    private readonly TimeslotService _timeslotService;

    public ExaminationService()
    {
        _examinationRepository = new ExaminationRepository();
        _timeslotService = new TimeslotService();
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

    public List<Examination> GetPostponableExaminations(SortedDictionary<DateTime, Doctor> earliestFreeTimeslotDoctors)
    {
        var postponableExaminations = new List<Examination>();
        foreach (var (timeslot, doctor) in earliestFreeTimeslotDoctors)
        {
            var doctorExaminations = _examinationRepository.GetAll(doctor);
            var nonUrgentUpcomingExaminations = doctorExaminations
                .Where(examination => examination.Start > DateTime.Now && !examination.Urgent)
                .OrderBy(examination => examination.Start).ToList();

            postponableExaminations.AddRange(nonUrgentUpcomingExaminations);
            if (postponableExaminations.Count >= 5)
                break;
        }

        if (postponableExaminations.Count > 5)
            postponableExaminations = postponableExaminations.Take(5).ToList();

        return postponableExaminations;
    }

    public bool IsPatientBusy(Patient patient, DateTime timeslot)
    {
        var patientExaminations = _examinationRepository.GetAll(patient);

        return patientExaminations.Any(examination => _timeslotService.AreDatesEqual(examination.Start, timeslot));
    }
}