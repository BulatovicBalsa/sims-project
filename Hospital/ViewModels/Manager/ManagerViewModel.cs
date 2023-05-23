using System.Collections.ObjectModel;

namespace Hospital.ViewModels.Manager;

public class ManagerViewModel : ViewModelBase
{
    private OrderTabViewModel _orderTabViewModel;

    public ManagerViewModel()
    {
        Children = new ObservableCollection<object>
        {
            new OrderTabViewModel(),
            new TransferTabViewModel(),
            new RenovationTabViewModel()
        };
    }


    public ObservableCollection<object> Children { get; }
}