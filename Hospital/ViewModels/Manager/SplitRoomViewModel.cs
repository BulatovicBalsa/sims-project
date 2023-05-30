using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Hospital.Models.Manager;
using Hospital.Repositories.Manager;
using Hospital.Scheduling;
using Hospital.Services.Manager;

namespace Hospital.ViewModels.Manager;

public class SplitRoomViewModel : ViewModelBase
{
    private readonly Room _roomToSplit;
    private BindingList<Room> _newRooms;
    private Room _room;
    private TimeRange _timeRange;
    private readonly BindingList<Transfer> _transfersToNewRooms;

    public SplitRoomViewModel(Room roomToSplit)
    {
        _room = new Room();
        _newRooms = new BindingList<Room>
        {
            new Room(),
            new Room()
        };
        _newRooms[0].Type = RoomType.ExaminationRoom;
        _newRooms[1].Type = RoomType.ExaminationRoom;
        _roomToSplit = roomToSplit;
        _timeRange = new TimeRange(DateTime.Now, DateTime.Now);
        TransfersToNewRooms = CreateTransfersToNewRooms();
        SplitRoomCommand = new RelayCommand<IClosable>(SplitRoom);
    }

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

    public TimeRange TimeRange
    {
        get => _timeRange;
        set
        {
            if (Equals(value, _timeRange)) return;
            _timeRange = value;
            OnPropertyChanged(nameof(TimeRange));
        }
    }

    public ICommand SplitRoomCommand { get; }

    public BindingList<Transfer> TransfersToNewRooms
    {
        get => _transfersToNewRooms;
        private init
        {
            if (Equals(value, _transfersToNewRooms)) return;
            _transfersToNewRooms = value;
            OnPropertyChanged(nameof(TransfersToNewRooms));
        }
    }

    private BindingList<Transfer> CreateTransfersToNewRooms()
    {
        var transfersToNewRooms = new List<Transfer>
        {
            new(_roomToSplit, _newRooms[0], DateTime.Now),
            new(_roomToSplit, _newRooms[1], DateTime.Now)
        };
        foreach (var equipment in _roomToSplit.GetEquipment())
            transfersToNewRooms.ForEach(transfer => transfer.AddItem(new TransferItem(equipment, 0)));
        return new BindingList<Transfer>(transfersToNewRooms);
    }

    private ComplexRenovation GetRenovation()
    {
        return new ComplexRenovation(new List<Room> { _roomToSplit }, _newRooms.ToList(),
            TimeRange, RoomRepository.Instance.GetWarehouse(),
            TransfersToNewRooms.ToList());
    }

    private void SplitRoom(IClosable window)
    {
        if (!Validate())
            return;
        var renovation = GetRenovation();
        var complexRenovationService = new ComplexRenovationService(new RoomScheduleService());
        if (!complexRenovationService.AddComplexRenovation(renovation))
            MessageBox.Show("The renovation can not be performed at the specified time");
        else
            window.Close();
    }

    private bool Validate()
    {
        if (!ValidateTimeRange()) return false;

        if (!IsEquipmentProperlyRedistributed()) return false;

        return true;
    }

    private bool ValidateTimeRange()
    {
        if (TimeRange.StartTime <= TimeRange.EndTime) return true;
        MessageBox.Show("Start time can not be after end time");
        return false;
    }

    private bool IsEquipmentProperlyRedistributed()
    {
        for (var i = 0; i < TransfersToNewRooms[0].Items.Count; i++)
        {
            var leftRoomItem = TransfersToNewRooms[0].Items[i];
            var rightRoomItem = TransfersToNewRooms[1].Items[i];
            if (leftRoomItem.Amount + rightRoomItem.Amount <=
                _roomToSplit.GetAvailableAmount(leftRoomItem.Equipment)) continue;
            MessageBox.Show(
                $"Attempted to redistribute more of {leftRoomItem.Equipment.Name} than there are available");
            return false;
        }

        return true;
    }
}