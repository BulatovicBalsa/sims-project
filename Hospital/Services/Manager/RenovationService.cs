using System.Collections.Generic;
using System.Linq;
using System.Timers;
using Hospital.Models.Manager;
using Hospital.Repositories.Manager;
using Hospital.Scheduling;

namespace Hospital.Services.Manager;

public class RenovationService
{
    private readonly RenovationRepository? _renovationRepository;
    private readonly RoomScheduleService _roomScheduleService;

    private readonly Timer _timer;

    public RenovationService()
    {
        _roomScheduleService = new RoomScheduleService();
        _renovationRepository = RenovationRepository.Instance;
        _timer = new Timer(1000);
        _timer.Enabled = true;
        _timer.AutoReset = true;
        _timer.Elapsed += (sender, args) => TryCompleteAllRenovations();
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
        foreach (var renovation in _renovationRepository?.GetAll().Where(renovation => renovation.TryComplete()) ??
                                   new List<Renovation>())
            if (renovation.TryComplete())
                RenovationRepository.Instance.Update(renovation);
    }
}