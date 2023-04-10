using System;
using System.Collections.Generic;

namespace Hospital.Models.Manager;

public class Room
{
    public enum RoomType
    {
        WAREHOUSE,
        OPERATING_ROOM,
        EXAMINATION_ROOM,
        WAITING_ROOM,
        WARD
    }

    public Room()
    {
        Id = Guid.NewGuid().ToString();
        Name = "";
    }

    public Room(string name, RoomType type)
    {
        Id = Guid.NewGuid().ToString();
        Name = name;
        Type = type;
    }

    public Room(string id, string name, RoomType type)
    {
        Id = id;
        Name = name;
        Type = type;
    }

    public string Id { get; set; }
    public string Name { get; set; }

    public RoomType Type { get; set; }

    public List<EquipmentItem> Equipment { get; set; }

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
            Equipment.Add(new EquipmentItem(equipment.Id, Id, amount));
    }
}