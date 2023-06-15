using Hospital.Core.PatientHealthcare.Models;
using Hospital.Core.PatientHealthcare.Repositories;
using Hospital.Injectors;
using Hospital.Serialization;

namespace Hospital.Core.PatientHealthcare.Services;

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