using System;
using System.Collections.Generic;
using Hospital.Repositories.Manager;

namespace Hospital.Models.Manager;

public class EquipmentOrder
{
    private const double _deliveryTimeInDays = 1;
    public bool PickedUp
    {
        get;
        set;
    }

    public EquipmentOrder()
    {
        Id = Guid.NewGuid().ToString();
        Items = new List<EquipmentOrderItem>();
        PickedUp = false;
    }

    public EquipmentOrder(DateTime deliveryDateTime)
    {
        DeliveryDateTime = deliveryDateTime;
        Id = Guid.NewGuid().ToString();
        Items = new List<EquipmentOrderItem>();
    }

    public EquipmentOrder(string id, DateTime deliveryDateTime, bool pickedUp)
    {
        Items = new List<EquipmentOrderItem>();
        Id = id;
        DeliveryDateTime = deliveryDateTime;
        PickedUp = pickedUp;
    }

    public List<EquipmentOrderItem> Items { get; set; }
    public string Id { get; set; }
    public DateTime DeliveryDateTime { get; set; }

    public bool Delivered => DeliveryDateTime <= DateTime.Now;

    public static EquipmentOrder CreateBlankOrder()
    {

        return new EquipmentOrder(DateTime.Now.AddDays(_deliveryTimeInDays));
    }


    public void AddOrUpdateItem(Equipment equipment, int amount)
    {
        var item = Items.Find(e => e.EquipmentId == equipment.Id);
        if (item == null)
            Items.Add(new EquipmentOrderItem(Id, amount, equipment));
        else
            item.Amount = amount;
    }

    public int GetAmount(Equipment equipment)
    {
        var item = Items.Find(e => e.EquipmentId == equipment.Id);
        if (item == null)
            return 0;

        return item.Amount;
    }

    public void AddOrUpdateItem(string equipmentId, int amount)
    {
        var item = Items.Find(e => e.EquipmentId == equipmentId);
        if (item == null)
            Items.Add(new EquipmentOrderItem(Id, amount, equipmentId));
        else
            item.Amount = amount;
    }

    public void PickUp(Room destination)
    {
        if (!Delivered) return;

        if (PickedUp) return;

        foreach (var item in Items)
            destination.SetAmount(item.Equipment, destination.GetAmount(item.Equipment) + item.Amount);

        PickedUp = true;
    }


}