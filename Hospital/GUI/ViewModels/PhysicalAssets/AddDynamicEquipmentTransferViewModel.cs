using System;
using System.ComponentModel;
using System.Linq;
using Hospital.Core.PhysicalAssets.Models;
using Hospital.Core.PhysicalAssets.Services;

namespace Hospital.GUI.ViewModels.PhysicalAssets;

public class AddDynamicEquipmentTransferViewModel : AddTransferViewModelBase
{
    private BindingList<Room> _destinationRooms;

    public AddDynamicEquipmentTransferViewModel()
    {
        _destinationRooms = new BindingList<Room>(RoomFilterService.GetRoomsLowOnDynamicEquipment());
    }

    public BindingList<Room> DestinationRooms
    {
        get => _destinationRooms;
        set
        {
            if (Equals(value, _destinationRooms)) return;
            _destinationRooms = value;
            OnPropertyChanged(nameof(DestinationRooms));
        }
    }

    protected override void UpdateEquipmentList()
    {
        base.UpdateEquipmentList();
        var availableDynamicEquipmentAtOrigin = Equipment
            .Where(equipment => equipment.Type == EquipmentType.DynamicEquipment).ToList();
        Equipment = new BindingList<Equipment>(availableDynamicEquipmentAtOrigin);
    }

    public override void SendTransfer(IClosable window)
    {
        Date = DateTime.Now.AddDays(1);
        base.SendTransfer(window);
    }
}