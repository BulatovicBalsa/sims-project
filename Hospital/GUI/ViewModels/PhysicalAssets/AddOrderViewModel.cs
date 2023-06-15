using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Hospital.Core.PhysicalAssets.Models;
using Hospital.Core.PhysicalAssets.Services;

namespace Hospital.GUI.ViewModels.PhysicalAssets;

public class AddOrderViewModel : ViewModelBase
{
    private BindingList<Equipment> _dynamicEquipmentRunningOut;

    private BindingList<EquipmentOrderItem> _items;

    private Equipment? _selectedEquipment;

    public AddOrderViewModel()
    {
        var filterService = new EquipmentFilterService();
        DynamicEquipmentRunningOut = new BindingList<Equipment>(filterService.GetDynamicEquipmentLowInWarehouse());
        Items = new BindingList<EquipmentOrderItem>();
        AddItemCommand = new RelayCommand(AddItem);
        SendOrderCommand = new RelayCommand<IClosable>(SendOrder);
    }

    public BindingList<Equipment> DynamicEquipmentRunningOut
    {
        get => _dynamicEquipmentRunningOut;
        set
        {
            if (_dynamicEquipmentRunningOut == value) return;
            _dynamicEquipmentRunningOut = value;
            OnPropertyChanged(nameof(DynamicEquipmentRunningOut));
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

    public BindingList<EquipmentOrderItem> Items
    {
        get => _items;
        set
        {
            if (_items == value) return;
            _items = value;
            OnPropertyChanged(nameof(Items));
        }
    }

    public ICommand AddItemCommand { get; set; }

    public ICommand SendOrderCommand { get; set; }

    public void AddItem()
    {
        if (SelectedEquipment == null) return;
        Items.Add(new EquipmentOrderItem("", 1, SelectedEquipment));
        DynamicEquipmentRunningOut.Remove(SelectedEquipment);
    }

    public void SendOrder(IClosable window)
    {
        if (ValidateOrder())
        {
            EquipmentOrderService.SendOrder(Items.ToList());
            window.Close();
        }
    }

    public bool ValidateOrder()
    {
        if (Items.Count > 0) return true;

        MessageBox.Show("An order must contain at least one item");
        return false;
    }
}