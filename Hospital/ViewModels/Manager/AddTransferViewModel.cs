﻿using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Hospital.Models.Manager;
using Hospital.Repositories.Manager;
using Hospital.Services.Manager;

namespace Hospital.ViewModels.Manager;

public class AddTransferViewModel : ViewModelBase
{
    private ICommand _addItemCommand;
    private BindingList<Equipment> _equipment;
    private BindingList<TransferItem> _items;
    private BindingList<Room> _rooms;
    private Room? _selectedDestination;


    private Equipment? _selectedEquipment;
    private Room? _selectedOrigin;
    private ICommand _sendTransferCommand;

    public AddTransferViewModel()
    {
        Rooms = new BindingList<Room>(RoomRepository.Instance.GetAll());
        Equipment = new BindingList<Equipment>(EquipmentRepository.Instance.GetAll());
        Items = new BindingList<TransferItem>();
        AddItemCommand = new RelayCommand(AddItem);
        SendTransferCommand = new RelayCommand<IClosable>(SendTransfer);
    }

    public BindingList<Equipment> Equipment
    {
        get => _equipment;
        set
        {
            if (Equals(value, _equipment)) return;
            _equipment = value;
            OnPropertyChanged(nameof(Equipment));
        }
    }

    public Equipment? SelectedEquipment
    {
        get => _selectedEquipment;
        set
        {
            if (_selectedEquipment == value) return;
            _selectedEquipment = value;
            OnPropertyChanged(nameof(SelectedEquipment));
        }
    }

    public BindingList<TransferItem> Items
    {
        get => _items;
        set
        {
            if (Equals(value, _items)) return;
            _items = value;
            OnPropertyChanged(nameof(Items));
        }
    }

    public Room? SelectedOrigin
    {
        get => _selectedOrigin;
        set
        {
            if (Equals(value, _selectedOrigin)) return;
            _selectedOrigin = value;
            OnPropertyChanged(nameof(SelectedOrigin));
        }
    }

    public Room? SelectedDestination
    {
        get => _selectedDestination;
        set
        {
            if (Equals(value, _selectedDestination)) return;
            _selectedDestination = value;
            OnPropertyChanged(nameof(SelectedDestination));
        }
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

    public ICommand AddItemCommand
    {
        get => _addItemCommand;
        set
        {
            if (Equals(value, _addItemCommand)) return;
            _addItemCommand = value;
            OnPropertyChanged(nameof(AddItemCommand));
        }
    }

    public ICommand SendTransferCommand
    {
        get => _sendTransferCommand;
        set
        {
            if (Equals(value, _sendTransferCommand)) return;
            _sendTransferCommand = value;
            OnPropertyChanged(nameof(SendTransferCommand));
        }
    }

    public void AddItem()
    {
        if (SelectedEquipment == null) return;
        Items.Add(new TransferItem("", SelectedEquipment, 0));
        Equipment.Remove(SelectedEquipment);
    }

    public void SendTransfer(IClosable window)
    {
        if (ValidateTransfer()) window.Close();
    }

    public bool ValidateTransfer()
    {
        if (Items.Count == 0)
        {
            MessageBox.Show("An order must contain at least one item");
            return false;
        }

        if (SelectedOrigin == null)
        {
            MessageBox.Show("Origin room not selected.");
            return false;
        }

        if (SelectedDestination == null)
        {
            MessageBox.Show("Destination room not selected");
            return false;
        }

        if (!TransferService.TrySendTransfer(SelectedOrigin, SelectedDestination, Items.ToList(),
                DateTime.Now.AddDays(1)))
        {
            MessageBox.Show(
                "Can not send more equipment than there are available for transfers. (Some equipment may have been reserved)");
            return false;
        }

        return true;
    }
}