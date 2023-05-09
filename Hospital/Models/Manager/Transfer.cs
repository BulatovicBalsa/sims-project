using System;
using System.Collections.Generic;

namespace Hospital.Models.Manager;

public class Transfer
{
    public Transfer()
    {
        Id = "";
        Origin = new Room();
        Destination = new Room();
        Items = new List<EquipmentTransferItem>();
        DeliveryDateTime = DateTime.Now;
        Delivered = false;
        Failed = false;
    }

    public Transfer(Room origin, Room destination, DateTime deliveryDateTime)
    {
        Id = Guid.NewGuid().ToString();
        Origin = origin;
        Destination = destination;
        Items = new List<EquipmentTransferItem>();
        DeliveryDateTime = deliveryDateTime;
        Delivered = false;
        Failed = false;
    }
    public Transfer(Room origin, Room destination, DateTime deliveryDateTime, List<EquipmentTransferItem> items)
    {
        Id = Guid.NewGuid().ToString();
        Origin = origin;
        Destination = destination;
        Items = items;
        DeliveryDateTime = deliveryDateTime;
        Delivered = false;
        Failed = false;
    }

    public string Id { get; set; }
    public Room Origin { get; set; }
    public Room Destination { get; set; }
    public List<EquipmentTransferItem> Items { get; set; }
    public DateTime DeliveryDateTime { get; set; }

    public bool Delivered { get; set; }
    public bool Failed { get; set; }

    public void AddItem(EquipmentTransferItem item)
    {
        Items.Add(item);
    } 

    public bool IsPossible()
    {
        return Origin.HasEnoughEquipment(this);
    }


    private bool IsReadyForDelivery()
    {
        return !Delivered && DeliveryDateTime <= DateTime.Now && !Failed;
    }

    public bool TryDeliver()
    {
        if (!IsReadyForDelivery())
            return false;

        if (!Origin.Send(this))
        {
            Failed = true;
            return false;
        }

        Destination.Receive(this);
        Delivered = true;
        return true;
    }
}