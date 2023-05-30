using System;
using System.ComponentModel;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Hospital.Models.Manager;
using Hospital.Repositories.Manager;
using Hospital.Views.Manager;

namespace Hospital.ViewModels.Manager;

public class RoomTabViewModel : ViewModelBase
{
    private BindingList<Room> _rooms;
    private Room? _selectedRoom;
    private BindingList<InventoryItem> _selectedRoomInventory;
    private readonly RelayCommand _splitRoomCommand;

    public RoomTabViewModel()
    {
        _rooms = new BindingList<Room>(RoomRepository.Instance.GetAll());
        _selectedRoomInventory = new BindingList<InventoryItem>();
        _selectedRoom = null;
        _splitRoomCommand = new RelayCommand(SplitRoom, IsRoomSplittingEnabled);
    }

    public BindingList<Room> Rooms
    {
        get => _rooms;
        set
        {
            if (Equals(value, _rooms)) return;
            _rooms = value;
            OnPropertyChanged(nameof(Rooms));
        }
    }

    public BindingList<InventoryItem> SelectedRoomInventory
    {
        get => _selectedRoomInventory;
        set
        {
            if (Equals(value, _selectedRoomInventory)) return;
            _selectedRoomInventory = value;
            OnPropertyChanged(nameof(SelectedRoomInventory));
        }
    }

    public Room? SelectedRoom
    {
        get => _selectedRoom;
        set
        {
            if (Equals(value, _selectedRoom)) return;
            _selectedRoom = value;
            SelectedRoomInventory = _selectedRoom != null
                ? new BindingList<InventoryItem>(_selectedRoom.Inventory)
                : new BindingList<InventoryItem>();
            _splitRoomCommand.RaiseCanExecuteChanged();
            OnPropertyChanged(nameof(SelectedRoom));
        }
    }

    public ICommand SplitRoomCommand => _splitRoomCommand;

    private bool IsRoomSplittingEnabled()
    {
        return SelectedRoom != null && SelectedRoom.Type != RoomType.Warehouse && SelectedRoom.DemolitionDate == null;
    }

    public void SplitRoom()
    {
        if (SelectedRoom == null) return;
        var dialog = new SplitRoom(SelectedRoom);
        dialog.Show();
        dialog.Closed += RefershRoomsOnFormClose;
    }

    private void RefershRoomsOnFormClose(object? sender, EventArgs e)
    {
        Rooms = new BindingList<Room>(RoomRepository.Instance.GetAll());
    }
}