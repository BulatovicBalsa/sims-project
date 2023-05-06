using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Models.Manager;
using Hospital.Repositories.Manager;

namespace Hospital.ViewModels.Manager
{
    public class OrderTabViewModel: ViewModelBase
    {
        public ObservableCollection<EquipmentOrder> Orders { get; }

        private EquipmentOrder _selectedOrder;
        private ObservableCollection<EquipmentOrderItem> _selectedOrderItems;

        public EquipmentOrder SelectedOrder
        {
            get => _selectedOrder;
            set
            {
                if (_selectedOrder != value)
                {
                    _selectedOrder = value;
                    SelectedOrderItems = value != null ? new ObservableCollection<EquipmentOrderItem>(SelectedOrder.Items) : new ObservableCollection<EquipmentOrderItem>();
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

        public OrderTabViewModel()
        { 
            Orders = new ObservableCollection<EquipmentOrder>(EquipmentOrderRepository.Instance.GetAll());
            SelectedOrderItems = new ObservableCollection<EquipmentOrderItem>();
        }
    }
}
