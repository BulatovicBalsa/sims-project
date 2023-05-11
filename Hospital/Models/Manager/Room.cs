using System;
using System.Collections.Generic;
using System.Linq;

namespace Hospital.Models.Manager;

public class Room
{
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
            where equipmentPlacement.Equipment.Type == EquipmentType.DynamicEquipment
            select equipmentPlacement).ToList();
    }

    public void ExpendEquipment(Equipment equipment, int amount)
    {
        var newAmount = GetAmount(equipment) - amount;
        if (newAmount >= 0)
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


    private EquipmentPlacement? GetPlacement(Equipment equipment)
    {
        return Equipment.Find(placement => placement.Equipment != null && placement.Equipment.Equals(equipment));
    }

    private int GetReservedAmount(Equipment equipment)
    {
        var placement = GetPlacement(equipment);
        return placement?.Reserved ?? 0;
    }

    public bool CanReserve(Equipment equipment, int amount)
    {
        return GetAmount(equipment) >= GetReservedAmount(equipment) + amount;
    }

    public bool HasEnoughEquipment(Transfer transfer)
    {
        return transfer.Items.All(item => CanReserve(item.Equipment, item.Amount));
    }

    private void Reserve(Equipment equipment, int amount)
    {
        var placement = GetPlacement(equipment);
        if (placement != null)
            placement.Reserved += amount;
    }

    public bool ReserveEquipment(Transfer transfer)
    {
        if (!HasEnoughEquipment(transfer))
            return false;

        transfer.Items.ForEach(item => Reserve(item.Equipment, item.Amount));

        return true;
    }

    private void ReleaseReserved(Equipment equipment, int amount)
    {
        var placement = GetPlacement(equipment);
        if (placement != null) placement.Reserved -= amount;
    }

    public bool Send(Transfer transfer)
    {
        foreach (var item in transfer.Items)
        {
            if (GetAmount(item.Equipment) < item.Amount)
                return false;
            ReleaseReserved(item.Equipment, item.Amount);
            ExpendEquipment(item.Equipment, item.Amount);
        }

        return true;
    }

    public void Receive(Transfer transfer)
    {
        transfer.Items.ForEach(item => SetAmount(item.Equipment, GetAmount(item.Equipment) + item.Amount));
    }
}