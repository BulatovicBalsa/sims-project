using GalaSoft.MvvmLight.Command;
using Hospital.Models.Manager;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace Hospital.ViewModels;

public class ChangeDynamicRoomEquipmentViewModel : ViewModelBase
{
    private ObservableCollection<EquipmentPlacement> _roomEquipments = new();

    public ChangeDynamicRoomEquipmentViewModel(Room room)
    {
        RoomEquipments = new ObservableCollection<EquipmentPlacement>(room.Equipment); //room.GetDynamicEquipment
        SaveCommand = new RelayCommand<Window>(Save);
    }

    public ObservableCollection<EquipmentPlacement> RoomEquipments
    {
        get => _roomEquipments;
        set
        {
            _roomEquipments = value;
            OnPropertyChanged(nameof(RoomEquipments));
        }
    }

    public ICommand SaveCommand { get; set; }

    private void Save(Window window)
    {
        window.DialogResult = true;
        //TODO: Substract used equipment from room equipment amounts
    }
}