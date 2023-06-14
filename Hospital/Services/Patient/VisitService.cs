using Hospital.Injectors;
using Hospital.Models.Patient;
using Hospital.Repositories.Patient;
using Hospital.Serialization;

namespace Hospital.Services;
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
