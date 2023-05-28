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
    }

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

    public static ComplexRenovation Schedule(List<Room> toDemolish, List<Room> toBuild, TimeRange time,
        Room leftoverEquipmentDestination, List<Transfer> transfersFromOldToNewRooms)
    {
        foreach (var room in toDemolish) room.DemolitionDate = time.EndTime;
        foreach (var transfer in transfersFromOldToNewRooms) transfer.Origin.TryReserveEquipment(transfer);
        foreach (var room in toBuild) room.CreationDate = time.EndTime;

        return new ComplexRenovation(toDemolish, toBuild, time, leftoverEquipmentDestination,
            transfersFromOldToNewRooms);
    }

    public bool TryComplete()
    {
        if (EndTime > DateTime.Now || Completed)
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