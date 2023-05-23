using System.Linq;
using Hospital.Models.Manager;
using Hospital.Repositories.Manager;
using Hospital.Scheduling;

namespace Hospital.Services.Manager;

public class RenovationService
{
    private readonly RenovationRepository? _renovationRepository;
    private readonly RoomScheduleService _roomScheduleService;

    public RenovationService()
    {
        _roomScheduleService = new RoomScheduleService();
        _renovationRepository = RenovationRepository.Instance;
    }

    public RenovationService(RoomScheduleService roomScheduleService)
    {
        _roomScheduleService = roomScheduleService;
    }

    public RenovationService(RoomScheduleService roomScheduleService, RenovationRepository? renovationRepository)
    {
        _roomScheduleService = roomScheduleService;
        _renovationRepository = renovationRepository;
    }

    public bool AddRenovation(Renovation renovation)
    {
        if (!_roomScheduleService.IsFree(renovation.Room, new TimeRange(renovation.BeginTime, renovation.EndTime)))
            return false;

        _renovationRepository?.Add(renovation);
        return true;
    }

    public void TryCompleteAllRenovations()
    {
        foreach (var renovation in RenovationRepository.Instance.GetAll().Where(renovation => renovation.TryComplete()))
        {
            if (renovation.TryComplete())
                continue;
            ; //TODO: Write changes
        }
    }
}