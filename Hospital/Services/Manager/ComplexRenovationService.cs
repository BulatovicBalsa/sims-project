using System.Collections.Generic;
using System.Timers;
using System.Windows.Controls.Ribbon.Primitives;
using Hospital.Models.Manager;
using Hospital.Repositories.Manager;

namespace Hospital.Services.Manager;

public class ComplexRenovationService
{
    private static Timer Timer = new Timer(1000);
    static ComplexRenovationService() {
         Timer.Enabled = true;
        Timer.AutoReset = true;
        Timer.Elapsed += ((sender, args) => TryCompleteAll());

    }
    private readonly List<ComplexRenovation> _complexRenovations;

    public ComplexRenovationService(RoomScheduleService roomScheduleService)
    {
        RoomScheduleService = roomScheduleService;
        _complexRenovations = new List<ComplexRenovation>();
    }

    public RoomScheduleService RoomScheduleService { get; }

    public bool AddComplexRenovation(ComplexRenovation complexRenovation)
    {
        if (!CanBePerformed(complexRenovation)) return false;

        complexRenovation.Schedule();

        RoomRepository.Instance.Add(complexRenovation.ToBuild);
        RenovationRepository.Instance.Add(complexRenovation.GetSimpleRenovations());

        _complexRenovations.Add(complexRenovation);
        ComplexRenovationRepository.Instance.Add(complexRenovation);
        return true;
    }

    public bool CanBePerformed(ComplexRenovation complexRenovation)
    {
        return RoomScheduleService.AreFreeFrom(complexRenovation.ToDemolish, complexRenovation.BeginTime) &&
               RoomScheduleService.HaveNoTransfersScheduledFrom(complexRenovation.ToDemolish,
                   complexRenovation.BeginTime) &&
               !AnySetForDemolition(complexRenovation.ToDemolish);
    }

    private bool IsSetForDemolition(Room room)
    {
        return _complexRenovations.Exists(complexRenovation => complexRenovation.WillDemolish(room));
    }

    private bool AnySetForDemolition(List<Room> rooms)
    {
        return rooms.Exists(IsSetForDemolition);
    }

    public static void TryCompleteAll()
    {
        foreach (var complexRenovation in ComplexRenovationRepository.Instance.GetAll())
        {
            if (!complexRenovation.TryComplete()) continue;
            complexRenovation.ToBuild.ForEach(room => RoomRepository.Instance.Update(room));
            complexRenovation.ToDemolish.ForEach(room => RoomRepository.Instance.Update(room));
        }
    }
}