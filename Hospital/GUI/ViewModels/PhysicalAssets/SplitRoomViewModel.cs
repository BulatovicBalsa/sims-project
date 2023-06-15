using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Hospital.Core.PhysicalAssets.Models;
using Hospital.Core.PhysicalAssets.Repositories;
using Hospital.Core.PhysicalAssets.Services;
using Hospital.Core.Scheduling;
using Hospital.Core.Scheduling.Services;

namespace Hospital.GUI.ViewModels.PhysicalAssets;

public class SplitRoomViewModel : ViewModelBase
{
    private readonly Room _roomToSplit;
    private readonly BindingList<Transfer> _transfersToNewRooms;
    private BindingList<Room> _newRooms;
    private Room _room;
    private TimeRange _timeRange;

    public SplitRoomViewModel(Room roomToSplit)
    {
        _room = new Room();
        _newRooms = new BindingList<Room>
        {
            new(),
            new()
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
        var renovation = GetRenovation();
        if (!Validate(renovation))
            return;
        var complexRenovationService = new ComplexRenovationService(new RoomScheduleService());
        if (!complexRenovationService.AddComplexRenovation(renovation))
            MessageBox.Show("The renovation can not be performed at the specified time");
        else
            window.Close();
    }

    private bool Validate(ComplexRenovation renovation)
    {
        if (!ValidateTimeRange()) return false;

        if (!IsEquipmentProperlyRedistributed(renovation)) return false;

        return true;
    }

    private bool ValidateTimeRange()
    {
        if (TimeRange.StartTime <= TimeRange.EndTime) return true;
        MessageBox.Show("Start time can not be after end time");
        return false;
    }

    private bool IsEquipmentProperlyRedistributed(ComplexRenovation renovation)
    {
        if (renovation.IsEquipmentProperlyRedistributed())
            return true;

        MessageBox.Show(
            "Attempted to redistribute more of some equipment than there are available");
        return false;
    }
}