using System.Collections.Generic;
using System.Linq;

namespace Hospital.Repositories.Patient;

using Hospital.Serialization;
using Models.Patient;
public class VisitRepository
{
    private const string FilePath = "../../../Data/visits.csv";

    public List<Visit> GetAll()
    {
        return CsvSerializer<Visit>.FromCSV(FilePath);
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

        CsvSerializer<Visit>.ToCSV(allVisits, FilePath);
    }
}
