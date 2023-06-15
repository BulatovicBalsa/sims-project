using System;
using System.Collections.Generic;
using System.Linq;
using Hospital.Core.PatientHealthcare.Repositories;
using Hospital.Injectors;
using Hospital.Serialization;

namespace Hospital.Core.PatientHealthcare.Models;

public class ExaminationChangesTracker
{
    private readonly ExaminationChangesTrackerRepository _examinationChangesTrackerRepository =
        new(SerializerInjector.CreateInstance<ISerializer<PatientExaminationLog>>());

    public ExaminationChangesTracker(ExaminationChangesTrackerRepository examinationChangesTrackerRepository)
    {
        _examinationChangesTrackerRepository = examinationChangesTrackerRepository;
    }

    public ExaminationChangesTracker()
    {
    }

    public void Add(PatientExaminationLog log)
    {
        _examinationChangesTrackerRepository.Add(log);
    }

    public int GetNumberOfChangeLogsForPatientInLast30Days(Patient patient)
    {
        IEnumerable<PatientExaminationLog> logs = _examinationChangesTrackerRepository.GetAll();
        var thirtyDaysAgo = DateTime.Now.AddDays(-30);
        return logs.Count(log => log.Patient.Equals(patient) && log.Timestamp > thirtyDaysAgo && !log.IsCreationLog);
    }

    public int GetNumberOfCreationLogsForPatientInLast30Days(Patient patient)
    {
        IEnumerable<PatientExaminationLog> logs = _examinationChangesTrackerRepository.GetAll();
        var thirtyDaysAgo = DateTime.Now.AddDays(-30);
        return logs.Count(log => log.Patient.Equals(patient) && log.Timestamp > thirtyDaysAgo && log.IsCreationLog);
    }
}