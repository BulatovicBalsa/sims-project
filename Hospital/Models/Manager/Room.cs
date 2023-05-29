using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Hospital.Models.Manager;

public class Room
{
    public Room()
    {
        Id = Guid.NewGuid().ToString();
        Name = "";
        Inventory = new List<InventoryItem>();
    }

    public Room(string name, RoomType type)
    {
        Id = Guid.NewGuid().ToString();
        Name = name;
        Type = type;
        Inventory = new List<InventoryItem>();
    }

    public Room(string id, string name, RoomType type)
    {
        Id = id;
        Name = name;
        Type = type;
        Inventory = new List<InventoryItem>();
    }

    [JsonProperty("Id")] public string Id { get; set; }

    [JsonProperty("Name")] public string Name { get; set; }

    [JsonProperty("Type")] public RoomType Type { get; set; }

    [JsonProperty("Inventory")] public List<InventoryItem> Inventory { get; set; }

    [JsonProperty("CreationDate")] public DateTime? CreationDate { get; set; }

    [JsonProperty("DemolitionDate")] public DateTime? DemolitionDate { get; set; }

    public int GetAmount(Equipment equipment)
    {
        var equipmentItem = Inventory.Find(equipmentItem => equipmentItem.EquipmentId == equipment.Id);
        return equipmentItem?.Amount ?? 0;
    }

    public void SetAmount(Equipment equipment, int amount)
    {
        var equipmentItem = Inventory.Find(equipmentItem => equipmentItem.EquipmentId == equipment.Id);
        if (equipmentItem != null)
            equipmentItem.Amount = amount;
        else
            Inventory.Add(new InventoryItem(equipment, Id, amount));
    }

    public List<Equipment> GetEquipment()
    {
        return (from equipmentPlacement in Inventory select equipmentPlacement.Equipment).ToList();
    }

    public List<InventoryItem> GetDynamicEquipmentAmounts()
    {
        return (from equipmentPlacement in Inventory
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


    private InventoryItem? GetInventoryItem(Equipment equipment)
    {
        return Inventory.Find(placement => placement.Equipment != null && placement.Equipment.Equals(equipment));
    }

    private int GetReservedAmount(Equipment equipment)
    {
        var placement = GetInventoryItem(equipment);
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

    private bool TryReserve(Equipment equipment, int amount)
    {
        if (!CanReserve(equipment, amount)) return false;
        var placement = GetInventoryItem(equipment);
        if (placement != null)
            placement.Reserved += amount;
        return true;
    }

    public bool TryReserveEquipment(Transfer transfer)
    {
        if (!HasEnoughEquipment(transfer))
            return false;

        transfer.Items.ForEach(item => TryReserve(item.Equipment, item.Amount));

        return true;
    }

    private void ReleaseReserved(Equipment equipment, int amount)
    {
        var placement = GetInventoryItem(equipment);
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
        transfer.Items.ForEach(item => AddEquipment(item.Equipment, item.Amount));
    }

    private void AddEquipment(Equipment equipment, int amount)
    {
        SetAmount(equipment, GetAmount(equipment) + amount);
    }

    public void SendAvailableInventory(Room destination)
    {
        foreach (var inventoryItem in Inventory)
        {
            destination.AddEquipment(inventoryItem.Equipment ?? throw new InvalidOperationException(),
                inventoryItem.Available);
            ExpendEquipment(inventoryItem.Equipment, inventoryItem.Available);
        }
    }
}