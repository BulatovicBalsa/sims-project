using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Hospital.Models.Manager;
using Hospital.Repositories.Manager;
using Hospital.Services.Manager;

namespace Hospital.ViewModels.Manager
{
    public class AddOrderFormViewModel : ViewModelBase
    {
        private BindingList<Equipment> _dynamicEquipmentRunningOut;
        public AddOrderFormViewModel()
        {
            var filterService = new EquipmentFilterService();
            DynamicEquipmentRunningOut = new BindingList<Equipment>(filterService.GetDynamicEquipmentLowInWarehouse());
            Items = new BindingList<EquipmentOrderItem>();
            AddItemCommand = new RelayCommand(AddItem);
            SendOrderCommand = new RelayCommand<Window>(SendOrder);
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

        private Equipment? _selectedEquipment;

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

        public void AddItem()
        {
            if (SelectedEquipment == null) return;
            Items.Add(new EquipmentOrderItem("", 1, SelectedEquipment));
            DynamicEquipmentRunningOut.Remove(SelectedEquipment);
            
        }

        private BindingList<EquipmentOrderItem> _items;

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

        public void SendOrder(Window window)
        {
            if (ValidateOrder())
            {
                EquipmentOrderService.SendOrder(Items.ToList());
                window.Close();
            }

        }

        public Boolean ValidateOrder()
        {
            if (Items.Count > 0)
            {
                return true;
            }

            MessageBox.Show("An order must contain at least one item");
            return false;
        }

        public ICommand AddItemCommand { get; set; }

        public ICommand SendOrderCommand { get; set; }
    }
}
