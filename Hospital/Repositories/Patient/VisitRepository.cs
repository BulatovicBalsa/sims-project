using System.Collections.Generic;
using System.Linq;

namespace Hospital.Repositories.Patient;

using Hospital.Serialization;
using Models.Patient;
public class VisitRepository
{
    private const string FilePath = "../../../Data/visits.csv";
    private readonly ISerializer<Visit> _serializer;

    public VisitRepository(ISerializer<Visit> serializer)
    {
        _serializer = serializer;
    }

    public List<Visit> GetAll()
    {
        return _serializer.Load(FilePath);
    }

    public List<Visit> GetByPatientId(string patientId)
    {
        var allVisits = GetAll();
        return allVisits.Where(visit => visit.PatientId == patientId).ToList();
    }

    public void Add(Visit visit)
    {
        var allVisits = GetAll();
        allVisits.Add(visit);

        _serializer.Save(allVisits, FilePath);
    }
}
