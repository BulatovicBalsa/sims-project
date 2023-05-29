using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Models.Manager;

namespace Hospital.ViewModels.Manager
{
    public class SplitRoomViewModel: ViewModelBase
    {
        private Room _room;
        private BindingList<Room> _newRooms;

        public Room Room
        {
            get => _room;
            set
            {
                if (Equals(value, _room)) return;
                _room = value;
                OnPropertyChanged(nameof(Room));
            }
        }

        public BindingList<Room> NewRooms
        {
            get => _newRooms;
            set
            {
                if (Equals(value, _newRooms)) return;
                _newRooms = value;
                OnPropertyChanged(nameof(NewRooms));
            }
        }

        public SplitRoomViewModel()
        {
            _room = new Room();
            _newRooms = new BindingList<Room>
            {
                new Room(),
                new Room()
            };
            _newRooms[0].Type = RoomType.ExaminationRoom;
            _newRooms[1].Type = RoomType.ExaminationRoom;
        }
    }
}
