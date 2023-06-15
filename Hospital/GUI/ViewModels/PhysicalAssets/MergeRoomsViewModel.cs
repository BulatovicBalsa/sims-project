using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Hospital.Core.PhysicalAssets.Models;
using Hospital.Core.Scheduling;
using Hospital.PhysicalAssets.Services;
using Hospital.Scheduling.Services;

namespace Hospital.GUI.ViewModels.PhysicalAssets;

public class MergeRoomsViewModel : ViewModelBase
{
    private readonly RelayCommand<IClosable> _mergeRoomsCommand;
    private Room _newRoom;
    private TimeRange _timeRange;

    public MergeRoomsViewModel(List<Room> toMerge)
    {
        _mergeRoomsCommand = new RelayCommand<IClosable>(MergeRooms);
        _timeRange = new TimeRange(DateTime.Now, DateTime.Now);
        ToMerge = toMerge;
        NewRoom = new Room();
        NewRoom.Type = RoomType.ExaminationRoom;
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

    public ICommand MergeRoomsCommand => _mergeRoomsCommand;

    private ComplexRenovation GetRenovation()
    {
        return new ComplexRenovation(ToMerge, new List<Room> { NewRoom }, TimeRange, NewRoom, new List<Transfer>());
    }

    private void MergeRooms(IClosable window)
    {
        if (!Validate()) return;
        var renovation = GetRenovation();
        var complexRenovationService = new ComplexRenovationService(new RoomScheduleService());
        if (!complexRenovationService.AddComplexRenovation(renovation))
            MessageBox.Show("The renovation can not be performed at the specified time");
        else
            window.Close();
    }

    private bool Validate()
    {
        if (TimeRange.StartTime <= TimeRange.EndTime) return true;
        MessageBox.Show("Start time can not be after end time");
        return false;
    }
}