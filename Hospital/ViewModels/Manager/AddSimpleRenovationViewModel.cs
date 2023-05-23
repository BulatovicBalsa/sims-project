using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Models.Manager;
using Hospital.Repositories.Manager;

namespace Hospital.ViewModels.Manager
{
    public class AddSimpleRenovationViewModel: ViewModelBase
    {
        private Room? _room;
        private DateTime _startTime;
        private DateTime _endTime;
        private ObservableCollection<Room>? _rooms;

        public Room? Room
        {
            get => _room;
            set
            {
                if (Equals(value, _room)) return;
                _room = value;
                OnPropertyChanged(nameof(Room));
                OnPropertyChanged(nameof(Room));
                OnPropertyChanged(nameof(Room));
            }
        }

        public DateTime StartTime
        {
            get => _startTime;
            set
            {
                if (value.Equals(_startTime)) return;
                _startTime = value;
                OnPropertyChanged(nameof(StartTime));
                OnPropertyChanged(nameof(StartTime));
            }
        }

        public DateTime EndTime
        {
            get => _endTime;
            set
            {
                if (value.Equals(_endTime)) return;
                _endTime = value;
                OnPropertyChanged(nameof(EndTime));
                OnPropertyChanged(nameof(EndTime));
            }
        }

        public ObservableCollection<Room>? Rooms
        {
            get => _rooms;
            set
            {
                if (Equals(value, _rooms)) return;
                _rooms = value;
                OnPropertyChanged(nameof(Rooms));
            }
        }

        public AddSimpleRenovationViewModel()
        {
            Rooms = new ObservableCollection<Room>(RoomRepository.Instance.GetAll().Where(room => room.Type != RoomType.Warehouse).ToList());
            
        }
    }
}
