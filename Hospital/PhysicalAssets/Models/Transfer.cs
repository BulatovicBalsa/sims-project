﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;

namespace Hospital.PhysicalAssets.Models;

public class Transfer : INotifyPropertyChanged
{
    private bool _delivered;
    private DateTime _deliveryDateTime;
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

    [JsonProperty("Id")] public string Id { get; set; }

    [JsonProperty("Origin")] public Room Origin { get; set; }

    [JsonProperty("Destination")] public Room Destination { get; set; }

    [JsonProperty("OriginId")] public string OriginId { get; set; }

    [JsonProperty("DestinationId")] public string DestinationId { get; set; }

    [JsonProperty("Items")] public List<TransferItem> Items { get; set; }

    [JsonProperty("DeliveryDateTime")]
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

    [JsonProperty("Delivered")]
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

    [JsonProperty("Failed")]
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

    public event PropertyChangedEventHandler? PropertyChanged;

    public void AddItem(TransferItem item)
    {
        item.TransferId = Id;
        Items.Add(item);
    }

    public bool IsPossible()
    {
        return Origin.HasEnoughEquipment(this);
    }


    public bool IsReadyForDelivery()
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