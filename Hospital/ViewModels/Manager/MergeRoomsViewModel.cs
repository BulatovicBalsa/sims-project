using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Hospital.Models.Manager;

namespace Hospital.ViewModels.Manager
{
    public class MergeRoomsViewModel: ViewModelBase
    {
        private RelayCommand<IClosable> _mergeRoomsCommand;
        private Room _newRoom;
        public List<Room> ToMerge { get; set; }

        public Room NewRoom
        {
            get => _newRoom;
            set
            {
                if (Equals(value, _newRoom)) return;
                _newRoom = value;
                OnPropertyChanged(nameof(NewRoom));
            }
        }

        public MergeRoomsViewModel(List<Room> toMerge)
        {
            _mergeRoomsCommand = new RelayCommand<IClosable>(MergeRooms);
            ToMerge = toMerge;
            NewRoom = new Room();
            NewRoom.Type = RoomType.ExaminationRoom;
            
        }

        public ICommand MergeRoomsCommand => _mergeRoomsCommand;

        private void MergeRooms(IClosable window)
        {
            window.Close();
        }
    }
}
