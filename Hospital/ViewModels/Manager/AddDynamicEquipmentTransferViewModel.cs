using System;
using System.ComponentModel;
using System.Linq;
using Hospital.Converters;
using Hospital.Models.Manager;
using Hospital.Services.Manager;

namespace Hospital.ViewModels.Manager;

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
        Equipment = new BindingList<Equipment>(Equipment
            .Where(equipment => equipment.Type == EquipmentType.DynamicEquipment).ToList());
    }

    public override void SendTransfer(IClosable window)
    {
        Date = DateTime.Now.AddDays(1);
        base.SendTransfer(window);
    }
}