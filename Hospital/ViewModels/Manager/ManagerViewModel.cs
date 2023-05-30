using System.Collections.ObjectModel;

namespace Hospital.ViewModels.Manager;

public class ManagerViewModel : ViewModelBase
{
    public ManagerViewModel()
    {
        Children = new ObservableCollection<object>
        {
            new OrderTabViewModel(),
            new TransferTabViewModel(),
            new RenovationTabViewModel(),
            new RoomTabViewModel()
        };
    }


    public ObservableCollection<object> Children { get; }
}