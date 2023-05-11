using System.Collections.ObjectModel;

namespace Hospital.ViewModels.Manager;

public class ManagerViewModel : ViewModelBase
{
    private OrderTabViewModel _orderTabViewModel;

    public ManagerViewModel()
    {
        Children = new ObservableCollection<object>();
        //Children.Add(new EquipmentTabViewModel());
        Children.Add(new OrderTabViewModel());
        Children.Add(new TransferTabViewModel());

        OrderTabViewModel = new OrderTabViewModel();
    }

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

    public ObservableCollection<object> Children { get; }
}