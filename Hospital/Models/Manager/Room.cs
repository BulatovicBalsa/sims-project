using System;
using System.Collections.Generic;
using System.Linq;

namespace Hospital.Models.Manager;

public class Room
{
    public enum RoomType
    {
        Warehouse,
        OperatingRoom,
        ExaminationRoom,
        WaitingRoom,
        Ward
    }

    public Room()
    {
        Id = Guid.NewGuid().ToString();
        Name = "";
        Equipment = new List<EquipmentPlacement>();
    }

    public Room(string name, RoomType type)
    {
        Id = Guid.NewGuid().ToString();
        Name = name;
        Type = type;
        Equipment = new List<EquipmentPlacement>();
    }

    public Room(string id, string name, RoomType type)
    {
        Id = id;
        Name = name;
        Type = type;
        Equipment = new List<EquipmentPlacement>();
    }

    public string Id { get; set; }
    public string Name { get; set; }

    public RoomType Type { get; set; }

    public List<EquipmentPlacement> Equipment { get; set; }

    public int GetAmount(Equipment equipment)
    {
        var equipmentItem = Equipment.Find(equipmentItem => equipmentItem.EquipmentId == equipment.Id);
        return equipmentItem?.Amount ?? 0;
    }

    public void SetAmount(Equipment equipment, int amount)
    {
        var equipmentItem = Equipment.Find(equipmentItem => equipmentItem.EquipmentId == equipment.Id);
        if (equipmentItem != null)
            equipmentItem.Amount = amount;
        else
            Equipment.Add(new EquipmentPlacement(equipment, Id, amount));
    }

    public List<Equipment> GetEquipment()
    {
        return (from equipmentPlacement in Equipment select equipmentPlacement.Equipment).ToList();
    }

    public List<EquipmentPlacement> GetDynamicEquipmentAmounts()
    {
        return (from equipmentPlacement in Equipment
            where equipmentPlacement.Equipment.Type == Models.Manager.Equipment.EquipmentType.DynamicEquipment
            select equipmentPlacement).ToList();
    }

    public void ExpendEquipment(Equipment equipment, int amount)
    {
        var newAmount = GetAmount(equipment) - amount;
        if(newAmount >= 0)
            SetAmount(equipment, newAmount);
    }

    public override string ToString()
    {
        return $"{Name}, {Type}";
    }

    public override bool Equals(object? obj)
    {
        if (obj is not Room objAsRoom) return false;
        return Id == objAsRoom.Id;
    }
}