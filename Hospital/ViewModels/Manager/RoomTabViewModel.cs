﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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
    private readonly RelayCommand _splitRoomCommand;
    private readonly RelayCommand<object> _mergeRoomsCommand;
    private ICommand _checkIfMergingIsEnabled;

    public ICommand CheckIfMergingIsEnabled
    {
        get => _checkIfMergingIsEnabled;
        set
        {
            if (Equals(value, _checkIfMergingIsEnabled)) return;
            _checkIfMergingIsEnabled = value;
            OnPropertyChanged(nameof(CheckIfMergingIsEnabled));
        }
    }

    public RoomTabViewModel()
    {
        _rooms = new BindingList<Room>(RoomRepository.Instance.GetAll());
        _selectedRoomInventory = new BindingList<InventoryItem>();
        _selectedRoom = null;
        _splitRoomCommand = new RelayCommand(SplitRoom, IsRoomSplittingEnabled);
        _mergeRoomsCommand = new RelayCommand<object>(MergeRooms, IsRoomMergingEnabled);
        CheckIfMergingIsEnabled = new RelayCommand(() => _mergeRoomsCommand.RaiseCanExecuteChanged());
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
            _splitRoomCommand.RaiseCanExecuteChanged();
            _mergeRoomsCommand.RaiseCanExecuteChanged();
            OnPropertyChanged(nameof(SelectedRoom));
        }
    }

    public ICommand SplitRoomCommand => _splitRoomCommand;

    public RelayCommand<object> MergeRoomsCommand => _mergeRoomsCommand;

    private bool IsRoomSplittingEnabled()
    {
        return SelectedRoom != null && SelectedRoom.Type != RoomType.Warehouse && SelectedRoom.DemolitionDate == null;
    }

    public void SplitRoom()
    {
        if (SelectedRoom == null) return;
        var dialog = new SplitRoom(SelectedRoom);
        dialog.Show();
        dialog.Closed += RefershRoomsOnFormClose;
    }

    private void MergeRooms(object selectedRooms)
    {
        var dialog = new MergeRooms(ConvertCommandParameter(selectedRooms));
        dialog.Show();
        dialog.Closed += RefershRoomsOnFormClose;
    }

    private bool IsWarehouseSelected(IList<object> selectedRooms)
    {
        return selectedRooms.Any(room => ((Room)room).Type == RoomType.Warehouse);
    }

    private List<Room> ConvertCommandParameter(object selectedRooms)
    {
        return ((IList<object>)selectedRooms).ToList().ConvertAll(room => ((Room)room));
    }

    private bool AreAnyRoomsSetForDemolitionSelected(IList<object> selectedRooms)
    {
        return selectedRooms.Any(room => (((Room)(room)).DemolitionDate) != null);
    }

    private bool IsRoomMergingEnabled(object selectedRooms)
    {
        if (selectedRooms == null) return false;
        var selectedRoomsList = (IList<object>)selectedRooms;
        return selectedRoomsList.Count == 2 && !IsWarehouseSelected(selectedRoomsList) && !AreAnyRoomsSetForDemolitionSelected(selectedRoomsList);

    }
    private void RefershRoomsOnFormClose(object? sender, EventArgs e)
    {
        Rooms = new BindingList<Room>(RoomRepository.Instance.GetAll());
    }
}