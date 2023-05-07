using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Hospital.Models.Manager;

public class EquipmentOrder : INotifyPropertyChanged
{
    private const double _deliveryTimeInDays = 1;

    private bool _delivered;
    private DateTime _deliveryDateTime;
    private string _id;
    private List<EquipmentOrderItem> _items;
    private bool _pickedUp;

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

    public bool PickedUp
    {
        get => _pickedUp;
        set
        {
            if (value == _pickedUp) return;
            _pickedUp = value;
            OnPropertyChanged();
        }
    }

    public List<EquipmentOrderItem> Items
    {
        get => _items;
        set
        {
            if (Equals(value, _items)) return;
            _items = value;
            OnPropertyChanged();
        }
    }

    public string Id
    {
        get => _id;
        set
        {
            if (value == _id) return;
            _id = value;
            OnPropertyChanged();
        }
    }

    public DateTime DeliveryDateTime
    {
        get => _deliveryDateTime;
        set
        {
            if (value.Equals(_deliveryDateTime)) return;
            _deliveryDateTime = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(Delivered));
        }
    }

    public bool Delivered
    {
        get
        {
            var deliveryDatePassed = DeliveryDateTime <= DateTime.Now;
            if (_delivered == deliveryDatePassed) return _delivered;
            _delivered = deliveryDatePassed;
            OnPropertyChanged();

            return _delivered;
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

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
            if (item.Equipment != null)
                destination.SetAmount(item.Equipment, destination.GetAmount(item.Equipment) + item.Amount);

        PickedUp = true;
    }

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        if (PropertyChanged != null)
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}