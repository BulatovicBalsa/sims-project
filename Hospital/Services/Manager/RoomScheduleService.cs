using System.Collections.Generic;
using System.Linq;
using Hospital.Models.Examination;
using Hospital.Models.Manager;
using Hospital.Repositories.Examination;
using Hospital.Repositories.Manager;
using Hospital.Scheduling;

namespace Hospital.Services.Manager;

public class RoomScheduleService
{
    private readonly List<Examination> _examinations;
    private readonly List<Renovation> _renovations;

    public RoomScheduleService()
    {
        _examinations = ExaminationRepository.Instance.GetAll();
        _renovations = RenovationRepository.Instance.GetAll();
    }

    public RoomScheduleService(List<Examination> examinations, List<Renovation> renovations)
    {
        _examinations = examinations;
        _renovations = renovations;
    }

    public bool IsFree(Room room, TimeRange range)
    {
        return !HasExaminationsScheduled(room, range) && !HasRenovationsScheduled(room, range);
    }

    public bool HasRenovationsScheduled(Room room, TimeRange range)
    {
        var renovationsInRoom = _renovations.Where(renovation => renovation.Room.Equals(room))
            .ToList();
        return renovationsInRoom.Exists(renovation => renovation.OverlapsWith(range.StartTime, range.EndTime));
    }

    public bool HasExaminationsScheduled(Room room, TimeRange range)
    {
        var examinationsInRoom = _examinations
            .Where(examination => examination.Room != null && examination.Room.Equals(room)).ToList();
        return examinationsInRoom.Exists(examination => range.OverlapsWith(examination.Start, examination.End));
    }
}