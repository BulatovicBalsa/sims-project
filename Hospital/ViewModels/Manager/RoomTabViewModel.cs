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
    private ICommand _splitRoomCommand;

    public RoomTabViewModel()
    {
        _rooms = new BindingList<Room>(RoomRepository.Instance.GetAll());
        _selectedRoomInventory = new BindingList<InventoryItem>();
        _selectedRoom = null;
        SplitRoomCommand = new RelayCommand(SplitRoom);
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
            OnPropertyChanged(nameof(SelectedRoom));
        }
    }

    public ICommand SplitRoomCommand
    {
        get => _splitRoomCommand;
        set
        {
            if (Equals(value, _splitRoomCommand)) return;
            _splitRoomCommand = value;
            OnPropertyChanged(nameof(SplitRoomCommand));
        }
    }

    public void SplitRoom()
    {
        var dialog = new SplitRoom();
        dialog.Show();
    }
}