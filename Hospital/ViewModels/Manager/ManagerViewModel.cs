using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Services.Manager;

namespace Hospital.ViewModels.Manager
{
    public class ManagerViewModel: ViewModelBase
    {
        ObservableCollection<object> _children;
        private OrderTabViewModel _orderTabViewModel;

        public OrderTabViewModel OrderTabViewModel
        {
            get => _orderTabViewModel;
            set
            {
                if (_orderTabViewModel != value)
                {
                    _orderTabViewModel = value;
                    OnPropertyChanged(nameof(OrderTabViewModel));
                }
            }
        }

        public ManagerViewModel()
        {
            _children = new ObservableCollection<object>();
            _children.Add(new EquipmentTabViewModel());
            _children.Add(new OrderTabViewModel());

            OrderTabViewModel = new OrderTabViewModel();
        }

        public ObservableCollection<object> Children { get { return _children; } }

    }
}
