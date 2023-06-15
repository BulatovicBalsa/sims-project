using Hospital.Injectors;
using Hospital.PatientHealthcare.Models;
using Hospital.PatientHealthcare.Repositories;
using Hospital.Serialization;

namespace Hospital.PatientHealthcare.Services;

public class VisitService
{
    private readonly VisitRepository _visitRepository;

    public VisitService()
    {
        _visitRepository = new VisitRepository(SerializerInjector.CreateInstance<ISerializer<Visit>>());
    }

    public void Add(Visit visit)
    {
        _visitRepository.Add(visit);
    }
}