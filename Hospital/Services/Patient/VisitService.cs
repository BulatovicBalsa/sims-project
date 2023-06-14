using Hospital.Models.Patient;
using Hospital.Repositories.Patient;

namespace Hospital.Services;
public class VisitService
{
    private readonly VisitRepository _visitRepository;

    public VisitService()
    {
        _visitRepository = new VisitRepository();
    }

    public void Add(Visit visit)
    {
        _visitRepository.Add(visit);
    }
}
