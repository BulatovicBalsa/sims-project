using System.Collections.Generic;
using Hospital.Core.PatientHealthcare.Models;
using Hospital.Serialization;

namespace Hospital.Core.PatientHealthcare.Repositories;

public class ExaminationChangesTrackerRepository
{
    private const string FilePath = "../../../Data/examinationChangesTrackerLogs.csv";
    private readonly ISerializer<PatientExaminationLog> _serializer;

    public ExaminationChangesTrackerRepository(ISerializer<PatientExaminationLog> serializer)
    {
        _serializer = serializer;
    }

    public List<PatientExaminationLog> GetAll()
    {
        return _serializer.Load(FilePath);
    }

    public void Add(PatientExaminationLog log)
    {
        var allLogs = GetAll();
        allLogs.Add(log);
        _serializer.Save(allLogs, FilePath);
    }

    public void DeleteAll()
    {
        _serializer.Save(new List<PatientExaminationLog>(), FilePath);
    }
}