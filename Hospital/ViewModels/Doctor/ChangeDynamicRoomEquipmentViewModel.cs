using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Hospital.Models.Manager;
using Hospital.Repositories.Manager;

namespace Hospital.ViewModels;

public class ChangeDynamicRoomEquipmentViewModel : ViewModelBase
{
    public class EquipmentExpenditure
    {
        public Equipment Equipment { get; set; }
        public int OriginalAmount { get; set; }
        public int Amount { get; set; }

        public EquipmentExpenditure(EquipmentPlacement equipmentPlacement)
        {
            Equipment = equipmentPlacement.Equipment ?? throw new InvalidOperationException();
            OriginalAmount = equipmentPlacement.Amount;
            Amount = 0;
        }
    }

    private ObservableCollection<EquipmentExpenditure> _expenditures = new();
    private ObservableCollection<EquipmentPlacement> _roomEquipments = new();
    private ICommand _saveCommand;
    private readonly Room _room;

    public ChangeDynamicRoomEquipmentViewModel(Room room)
    {
        _room = room;
        foreach (var equipmentPlacement in room.GetDynamicEquipmentAmounts())
            Expenditures.Add(new EquipmentExpenditure(equipmentPlacement));

        RoomEquipments =
            new ObservableCollection<EquipmentPlacement>(room.GetDynamicEquipmentAmounts()); //room.GetDynamicEquipment
        SaveCommand = new RelayCommand<Window>(Save);
    }

    public ObservableCollection<EquipmentPlacement> RoomEquipments
    {
        get => _roomEquipments;
        set
        {
            _roomEquipments = value;
            OnPropertyChanged(nameof(RoomEquipments));
            OnPropertyChanged(nameof(RoomEquipments));
        }
    }

    public ICommand SaveCommand
    {
        get => _saveCommand;
        set
        {
            if (Equals(value, _saveCommand)) return;
            _saveCommand = value;
            OnPropertyChanged(nameof(SaveCommand));
        }
    }

    public ObservableCollection<EquipmentExpenditure> Expenditures
    {
        get => _expenditures;
        set
        {
            if (Equals(value, _expenditures)) return;
            _expenditures = value;
            OnPropertyChanged(nameof(Expenditures));
        }
    }

    private string ValidateInput(Window window)
    {
        foreach (var expenditure in Expenditures)
        { 
            if (expenditure.OriginalAmount < expenditure.Amount)
                return
                    $"It is not possible to spend more of {expenditure.Equipment.Name} than there currently are.";
        }

        return "";
    }


    private void ExpendEquipment()
    {
        foreach (var expenditure in Expenditures)
        { 
            _room.ExpendEquipment(expenditure.Equipment, expenditure.Amount);
        }

    }

    private void SaveEquipmentAmountChanges()
    {
        foreach (var equipmentPlacement in _room.Equipment)
        {
            EquipmentPlacementRepository.Instance.Update(equipmentPlacement);
        }
    }

    private void Save(Window window)
    {
        var errorMessage = ValidateInput(window);
        if (!string.IsNullOrEmpty(errorMessage))
        {
            MessageBox.Show(errorMessage);
            return;
        }

        ExpendEquipment(); 
        SaveEquipmentAmountChanges();

        window.DialogResult = true;
    }
}