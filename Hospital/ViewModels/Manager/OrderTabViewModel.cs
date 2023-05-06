using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Models.Manager;
using Hospital.Repositories.Manager;
using Hospital.Services.Manager;

namespace Hospital.ViewModels.Manager
{
    public class OrderTabViewModel: ViewModelBase
    {
        private BindingList<EquipmentOrder> _orders;
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
            EquipmentOrderService.AttemptPickUpOfAllOrders();
            Console.WriteLine("Constructed OrderTabViewModel");
            Orders = new BindingList<EquipmentOrder>(EquipmentOrderRepository.Instance.GetAll());
            Orders.ListChanged += (sender, _) => OnPropertyChanged(nameof(Orders));
            SelectedOrderItems = new ObservableCollection<EquipmentOrderItem>();
        }
    }
}
