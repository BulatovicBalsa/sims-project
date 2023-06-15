using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Hospital.PhysicalAssets.Models;
using Hospital.PhysicalAssets.Repositories;
using Hospital.PhysicalAssets.Services;
using Hospital.Scheduling;
using Hospital.Scheduling.Services;

namespace Hospital.GUI.ViewModels.PhysicalAssets;

public class AddSimpleRenovationViewModel : ViewModelBase
{
    private DateTime? _endTime;
    private Room? _room;
    private ObservableCollection<Room>? _rooms;
    private DateTime? _startTime;

    public AddSimpleRenovationViewModel()
    {
        Rooms = new ObservableCollection<Room>(RoomRepository.Instance.GetAll()
            .Where(room => room.Type != RoomType.Warehouse).ToList());
        SubmitCommand = new RelayCommand<IClosable>(Submit);
    }

    public Room? Room
    {
        get => _room;
        set
        {
            if (Equals(value, _room)) return;
            _room = value;
            OnPropertyChanged(nameof(Room));
        }
    }

    public DateTime? StartTime
    {
        get => _startTime;
        set
        {
            if (value.Equals(_startTime)) return;
            _startTime = value;
            OnPropertyChanged(nameof(StartTime));
        }
    }

    public DateTime? EndTime
    {
        get => _endTime;
        set
        {
            if (value.Equals(_endTime)) return;
            _endTime = value;
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

    public ICommand SubmitCommand { get; set; }


    public void Submit(IClosable window)
    {
        if (RenovationInvalid()) return;
        AddRenovation();
        window.Close();
    }

    private void AddRenovation()
    {
        var renovationService = new RenovationService();

        if (StartTime != null && EndTime != null && Room != null)
            renovationService.AddRenovation(new Renovation((DateTime)StartTime, (DateTime)EndTime, Room));
    }

    private bool RenovationInvalid()
    {
        if (Room == null || StartTime == null || EndTime == null)
        {
            MessageBox.Show("Some fields have not been entered.", "Error", MessageBoxButton.OK, MessageBoxImage.Error,
                MessageBoxResult.None);
            return true;
        }

        if (StartTime > EndTime)
        {
            MessageBox.Show("Start date can not be after end date.", "Error", MessageBoxButton.OK,
                MessageBoxImage.Error, MessageBoxResult.None);
            return true;
        }

        var roomScheduleService = new RoomScheduleService();
        if (roomScheduleService.IsFree(Room, new TimeRange((DateTime)StartTime, (DateTime)EndTime))) return false;

        MessageBox.Show("Room is not free during the specified time.", "Error", MessageBoxButton.OK,
            MessageBoxImage.Error, MessageBoxResult.None);
        return true;
    }
}