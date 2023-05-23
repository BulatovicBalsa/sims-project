using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Hospital.Models.Manager;
using Hospital.Repositories.Manager;

namespace Hospital.ViewModels;

public class RoomInventoryViewModel : ViewModelBase
{
    public class EquipmentExpenditure
    {
        public Equipment Equipment { get; set; }
        public int OriginalAmount { get; set; }
        public int Amount { get; set; }

        public EquipmentExpenditure(InventoryItem inventoryItem)
        {
            Equipment = inventoryItem.Equipment ?? throw new InvalidOperationException();
            OriginalAmount = inventoryItem.Amount;
            Amount = 0;
        }
    }

    private ObservableCollection<EquipmentExpenditure> _expenditures = new();
    private ObservableCollection<InventoryItem> _roomEquipments = new();
    private ICommand _saveCommand;
    private readonly Room _room;

    public RoomInventoryViewModel(Room room)
    {
        _room = room;
        foreach (var equipmentPlacement in room.GetDynamicEquipmentAmounts())
            Expenditures.Add(new EquipmentExpenditure(equipmentPlacement));

        RoomEquipments =
            new ObservableCollection<InventoryItem>(room.GetDynamicEquipmentAmounts());
        SaveCommand = new RelayCommand<Window>(Save);
    }

    public ObservableCollection<InventoryItem> RoomEquipments
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

    private string ValidateInput()
    {
        var invalidExpenditures = Expenditures.Where(expenditure => expenditure.OriginalAmount < expenditure.Amount).ToList();
        if (!invalidExpenditures.Any()) return "";
        var errorMessage = $"It is not possible to spend more of {invalidExpenditures.First().Equipment.Name} than there currently are.";
        return errorMessage;

    }


    private void ExpendEquipment()
    {
        Expenditures.ToList().ForEach(expenditure => _room.ExpendEquipment(expenditure.Equipment, expenditure.Amount));
    }

    private void Save(Window window)
    {
        var errorMessage = ValidateInput();
        if (!string.IsNullOrEmpty(errorMessage))
        {
            MessageBox.Show(errorMessage);
            return;
        }

        ExpendEquipment(); 
        RoomRepository.Instance.Update(_room);

        window.DialogResult = true;
    }
}