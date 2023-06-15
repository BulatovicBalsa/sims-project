using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Hospital.Core.PhysicalAssets.Models;
using Hospital.Core.PhysicalAssets.Repositories;
using Hospital.GUI.Views.PhysicalAssets;

namespace Hospital.GUI.ViewModels.PhysicalAssets;

public class OrderTabViewModel : ViewModelBase
{
    private BindingList<EquipmentOrder> _orders;

    private EquipmentOrder _selectedOrder;
    private ObservableCollection<EquipmentOrderItem> _selectedOrderItems;

    public OrderTabViewModel()
    {
        Orders = new BindingList<EquipmentOrder>(EquipmentOrderRepository.Instance.GetAll());
        Orders.ListChanged += (sender, _) => OnPropertyChanged(nameof(Orders));
        SelectedOrderItems = new ObservableCollection<EquipmentOrderItem>();
        OpenAddOrderFormCommand = new RelayCommand(OpenAddOrderForm);
    }

    public BindingList<EquipmentOrder> Orders
    {
        get => _orders;
        set
        {
            if (_orders == value) return;
            _orders = value;
            OnPropertyChanged(nameof(Orders));
        }
    }

    public EquipmentOrder SelectedOrder
    {
        get => _selectedOrder;
        set
        {
            if (_selectedOrder != value)
            {
                _selectedOrder = value;
                SelectedOrderItems = value != null
                    ? new ObservableCollection<EquipmentOrderItem>(SelectedOrder.Items)
                    : new ObservableCollection<EquipmentOrderItem>();
                OnPropertyChanged(nameof(SelectedOrder));
            }
        }
    }

    public ObservableCollection<EquipmentOrderItem> SelectedOrderItems
    {
        get => _selectedOrderItems;
        set
        {
            if (_selectedOrderItems != value)
            {
                _selectedOrderItems = value;
                OnPropertyChanged(nameof(SelectedOrderItems));
            }
        }
    }


    public ICommand OpenAddOrderFormCommand { get; set; }

    private void RefreshOrdersOnFormClose(object? sender, EventArgs eventArgs)
    {
        Orders = new BindingList<EquipmentOrder>(EquipmentOrderRepository.Instance.GetAll());
    }

    public void OpenAddOrderForm()
    {
        var addOrderForm = new AddOrder();
        addOrderForm.Closed += RefreshOrdersOnFormClose;
        addOrderForm.Show();
    }
}