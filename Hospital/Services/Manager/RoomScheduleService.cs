using System;
using System.Collections.Generic;
using System.Linq;
using Hospital.Models.Examination;
using Hospital.Models.Manager;
using Hospital.Repositories.Examinaton;
using Hospital.Repositories.Manager;
using Hospital.Scheduling;

namespace Hospital.Services.Manager;

public class RoomScheduleService
{
    private readonly List<Examination> _examinations;
    private readonly List<Renovation> _renovations;
    private readonly List<Transfer> _transfers;

    public RoomScheduleService()
    {
        _examinations = ExaminationRepository.Instance.GetAll();
        _renovations = RenovationRepository.Instance.GetAll();
        _transfers = TransferRepository.Instance.GetAll();
    }

    public RoomScheduleService(List<Examination> examinations, List<Renovation> renovations)
    {
        _examinations = examinations;
        _renovations = renovations;
        _transfers = new List<Transfer>();
    }

    public RoomScheduleService(List<Examination> examinations, List<Renovation> renovations, List<Transfer> transfers)
    {
        _examinations = examinations;
        _renovations = renovations;
        _transfers = transfers;
    }

    public bool IsFree(Room room, TimeRange range)
    {
        return !HasExaminationsScheduled(room, range) && !HasRenovationsScheduled(room, range);
    }

    public bool IsFreeFrom(Room room, DateTime date)
    {
        var untilEndOfTime = new TimeRange(date, DateTime.MaxValue);
        return !HasExaminationsScheduled(room, untilEndOfTime) && !HasRenovationsScheduled(room, untilEndOfTime);
    }

    public bool AreFreeFrom(List<Room> rooms, DateTime date)
    {
        return rooms.All(room => IsFreeFrom(room, date));
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

    public bool HasTransfersScheduledFrom(Room room, DateTime date)
    {
        var transfersInRoom = _transfers
            .Where(transfer => transfer.Origin.Equals(room) || transfer.Destination.Equals(room)).ToList();
        return transfersInRoom.Exists(transfer => transfer.DeliveryDateTime > date);
    }

    public bool HaveNoTransfersScheduledFrom(List<Room> rooms, DateTime date)
    {
        return rooms.All(room => !HasTransfersScheduledFrom(room, date));
    }
}