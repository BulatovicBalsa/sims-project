using System.Collections.Generic;
using Hospital.Models.Manager;
using Hospital.Repositories.Manager;

namespace Hospital.Services.Manager;

public class ComplexRenovationService
{
    private List<ComplexRenovation> complexRenovations;

    public ComplexRenovationService(RoomScheduleService roomScheduleService)
    {
        RoomScheduleService = roomScheduleService;
        complexRenovations = new List<ComplexRenovation>();
    }

    public RoomScheduleService RoomScheduleService { get; }

    public bool AddComplexRenovation(ComplexRenovation complexRenovation)
    {
        if (!CanBePerformed(complexRenovation)) return false;

        complexRenovation.Schedule();

        RoomRepository.Instance.Add(complexRenovation.ToBuild);
        RenovationRepository.Instance.Add(complexRenovation.GetSimpleRenovations());

        complexRenovations.Add(complexRenovation);
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
        return complexRenovations.Exists(complexRenovation => complexRenovation.WillDemolish(room));
    }

    private bool AnySetForDemolition(List<Room> rooms)
    {
        return rooms.Exists(IsSetForDemolition);
    }
}