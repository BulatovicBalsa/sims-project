using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Hospital.Models.Manager;

public class Transfer: INotifyPropertyChanged
{
    private DateTime _deliveryDateTime;
    private bool _delivered;
    private bool _failed;

    public Transfer()
    {
        Id = "";
        Origin = new Room();
        Destination = new Room();
        OriginId = "";
        DestinationId = "";
        Items = new List<TransferItem>();
        DeliveryDateTime = DateTime.Now;
        Delivered = false;
        Failed = false;
    }

    public Transfer(Room origin, Room destination, DateTime deliveryDateTime)
    {
        Id = Guid.NewGuid().ToString();
        Origin = origin;
        Destination = destination;
        OriginId = origin.Id;
        DestinationId = destination.Id;
        Items = new List<TransferItem>();
        DeliveryDateTime = deliveryDateTime;
        Delivered = false;
        Failed = false;
    }

    public Transfer(Room origin, Room destination, DateTime deliveryDateTime, List<TransferItem> items)
    {
        Id = Guid.NewGuid().ToString();
        Origin = origin;
        Destination = destination;
        OriginId = origin.Id;
        DestinationId = destination.Id;
        Items = items;
        DeliveryDateTime = deliveryDateTime;
        Delivered = false;
        Failed = false;
    }

    public string Id { get; set; }
    public Room Origin { get; set; }
    public Room Destination { get; set; }
    public string OriginId { get; set; }
    public string DestinationId { get; set; }
    public List<TransferItem> Items { get; set; }

    public DateTime DeliveryDateTime
    {
        get => _deliveryDateTime;
        set
        {
            if (value.Equals(_deliveryDateTime)) return;
            _deliveryDateTime = value;
            OnPropertyChanged();
        }
    }

    public bool Delivered
    {
        get => _delivered;
        set
        {
            if (value == _delivered) return;
            _delivered = value;
            OnPropertyChanged();
        }
    }

    public bool Failed
    {
        get => _failed;
        set
        {
            if (value == _failed) return;
            _failed = value;
            OnPropertyChanged();
        }
    }

    public void AddItem(TransferItem item)
    {
        item.TransferId = Id;
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

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}