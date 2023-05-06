using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Hospital.Models.Manager;

public class EquipmentOrderItem: INotifyPropertyChanged
{
    private int _amount;

    public EquipmentOrderItem()
    {
        Amount = 0;
        EquipmentId = "";
        OrderId = "";
    }

    public EquipmentOrderItem(string orderId, int amount, string equipmentId)
    {
        Amount = amount;
        EquipmentId = equipmentId;
        OrderId = orderId;
    }

    public EquipmentOrderItem(string orderId, int amount, Equipment equipment)
    {
        Amount = amount;
        EquipmentId = equipment.Id;
        Equipment = equipment;
        OrderId = orderId;
    }

    public int Amount
    {
        get => _amount;
        set
        {
            if (value == _amount) return;
            _amount = value;
            OnPropertyChanged();
        }
    }

    public string EquipmentId { get; set; }

    public string OrderId { get; set; }

    public Equipment? Equipment { get; set; }

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