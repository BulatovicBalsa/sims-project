using GalaSoft.MvvmLight.Command;
using Hospital.Coordinators;
using Hospital.Models.Manager;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Hospital.ViewModels
{
    public class ChangeDynamicalRoomEquipmentViewModel : ViewModelBase
    {
        private ObservableCollection<EquipmentPlacement> _roomEquipments = new();

        public ObservableCollection<EquipmentPlacement> RoomEquipments
        {
            get { return _roomEquipments; }
            set { _roomEquipments = value; OnPropertyChanged(nameof(RoomEquipments)); }
        }

        public ICommand SaveCommand { get; set; }

        public ChangeDynamicalRoomEquipmentViewModel(Room room)
        {
            RoomEquipments = new ObservableCollection<EquipmentPlacement>(room.Equipment); //room.GetDynamicalEquipment
            SaveCommand = new RelayCommand<Window>(Save);
        }

        private void Save(Window window)
        {
            window.DialogResult = true;
            //TODO: Substract used equipment from room equipment amounts
        }
    }
}
