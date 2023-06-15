using System;
using System.Collections.Generic;
using System.Linq;
using Hospital.Scheduling;
using Newtonsoft.Json;

namespace Hospital.PhysicalAssets.Models;

public class ComplexRenovation
{
    public ComplexRenovation(List<Room> toDemolish, List<Room> toBuild, TimeRange timeRange,
        Room leftoverEquipmentDestination, List<Transfer> transfersFromOldToNewRooms)
    {
        ToDemolish = toDemolish;
        ToBuild = toBuild;
        Time = timeRange;
        LeftoverEquipmentDestination = leftoverEquipmentDestination;
        TransfersFromOldToNewRooms = transfersFromOldToNewRooms;
        Completed = false;
        Scheduled = false;
        Id = Guid.NewGuid().ToString();
    }

    public Room LeftoverEquipmentDestination { get; set; }

    public string Id { get; }

    [JsonProperty("Scheduled")] public bool Scheduled { get; private set; }

    [JsonProperty("TransfersFromOldToNewRooms")]
    public List<Transfer> TransfersFromOldToNewRooms { get; }

    [JsonProperty("Time")] public TimeRange Time { get; private set; }

    [JsonProperty("BeginTime")]
    public DateTime BeginTime
    {
        get => Time.StartTime;
        set => Time.EndTime = value;
    }

    [JsonProperty("EndTime")]
    public DateTime EndTime
    {
        get => Time.EndTime;
        set => Time.EndTime = value;
    }

    [JsonProperty("ToDemolish")] public List<Room> ToDemolish { get; set; }

    [JsonProperty("ToBuild")] public List<Room> ToBuild { get; set; }

    [JsonProperty("Completed")] public bool Completed { get; private set; }

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

    public bool IsEquipmentProperlyRedistributed()
    {
        var transferItems = TransfersFromOldToNewRooms.Select(transfer => transfer.Items).SelectMany(x => x).ToList();
        var transferItemsByEquipment = from transferItem in transferItems
            group transferItem by transferItem.Equipment
            into equipmentGroup
            select equipmentGroup;
        return transferItemsByEquipment.All(grouping =>
            grouping.Sum(item => item.Amount) <= ToDemolish.Sum(room => room.GetAvailableAmount(grouping.Key)));
    }


    private void MoveEquipmentToNewRooms()
    {
        foreach (var transfer in TransfersFromOldToNewRooms) transfer.TryDeliver();
    }

    private void MoveLeftoverEquipment()
    {
        foreach (var oldRoom in ToDemolish) oldRoom.SendAvailableInventory(LeftoverEquipmentDestination);
    }

    public bool WillDemolish(Room room)
    {
        return ToDemolish.Contains(room);
    }
}