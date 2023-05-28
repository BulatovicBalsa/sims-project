using System;
using System.Collections.Generic;
using System.Linq;
using Hospital.Scheduling;

namespace Hospital.Models.Manager;

public class ComplexRenovation
{
    private readonly Room _leftoverEquipmentDestination;
    private readonly TimeRange _time;

    public ComplexRenovation(List<Room> toDemolish, List<Room> toBuild, TimeRange time,
        Room leftoverEquipmentDestination, List<Transfer> transfersFromOldToNewRooms)
    {
        ToDemolish = toDemolish;
        ToBuild = toBuild;
        _time = time;
        _leftoverEquipmentDestination = leftoverEquipmentDestination;
        TransfersFromOldToNewRooms = transfersFromOldToNewRooms;
        Completed = false;
        Scheduled = false;
    }

    public bool Scheduled { get; private set; }

    public List<Transfer> TransfersFromOldToNewRooms { get; }

    public TimeRange Time { get; set; }

    public DateTime BeginTime
    {
        get => _time.StartTime;
        set => _time.EndTime = value;
    }

    public DateTime EndTime
    {
        get => _time.EndTime;
        set => _time.EndTime = value;
    }

    public List<Room> ToDemolish { get; }

    public List<Room> ToBuild { get; }

    public bool Completed { get; private set; }

    public void Schedule()
    {
        Scheduled = true;
        foreach (var room in ToDemolish) room.DemolitionDate = EndTime;
        foreach (var transfer in TransfersFromOldToNewRooms) transfer.Origin.TryReserveEquipment(transfer);
        foreach (var room in ToBuild) room.CreationDate = EndTime;
    }

    public bool TryComplete()
    {
        if (EndTime > DateTime.Now || Completed || !Scheduled)
            return false;

        MoveEquipmentToNewRooms();

        MoveLeftoverEquipment();

        Completed = true;
        return true;
    }

    public List<Renovation> GetSimpleRenovations()
    {
        var renovations = ToDemolish.ToList().Select(room => new Renovation(BeginTime, EndTime, room)).ToList();
        renovations.AddRange(ToBuild.Select(room => new Renovation(BeginTime, EndTime, room)));
        return renovations;
    }

    private void MoveEquipmentToNewRooms()
    {
        foreach (var transfer in TransfersFromOldToNewRooms) transfer.TryDeliver();
    }

    private void MoveLeftoverEquipment()
    {
        foreach (var oldRoom in ToDemolish) oldRoom.SendAvailableInventory(_leftoverEquipmentDestination);
    }

    public bool WillDemolish(Room room)
    {
        return ToDemolish.Contains(room);
    }
}