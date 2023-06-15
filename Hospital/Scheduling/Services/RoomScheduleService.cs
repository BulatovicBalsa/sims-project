﻿using System;
using System.Collections.Generic;
using System.Linq;
using Hospital.PatientHealthcare.Models;
using Hospital.PatientHealthcare.Repositories;
using Hospital.PhysicalAssets.Models;
using Hospital.PhysicalAssets.Repositories;

namespace Hospital.Scheduling.Services;

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

    public RoomScheduleService(List<Examination> examinations, List<Renovation> renovations,
        List<Transfer>? transfers = null)
    {
        _examinations = examinations;
        _renovations = renovations;
        _transfers = transfers ?? new List<Transfer>();
    }

    public bool IsFree(Room room, TimeRange range)
    {
        return !HasExaminationsScheduled(room, range) && !HasRenovationsScheduled(room, range) &&
               room.WillExistDuring(range);
    }

    public bool IsFreeFrom(Room room, DateTime date)
    {
        var untilEndOfTime = new TimeRange(date, DateTime.MaxValue);
        return IsFree(room, untilEndOfTime);
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
        return examinationsInRoom.Exists(examination => range.DoesOverlapWith(examination.Start, examination.End));
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