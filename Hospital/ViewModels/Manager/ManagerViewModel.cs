using System.Collections.ObjectModel;

namespace Hospital.ViewModels.Manager;

public class ManagerViewModel : ViewModelBase
{
    public ManagerViewModel()
    {
        OrderTabViewModel = new OrderTabViewModel();
        TransferTabViewModel = new TransferTabViewModel();
        RenovationTabViewModel = new RenovationTabViewModel();
        RoomTabViewModel = new RoomTabViewModel();
        HospitalSurveyTabViewModel = new HospitalSurveyTabViewModel();
    }


    public OrderTabViewModel OrderTabViewModel { get; }
    public TransferTabViewModel TransferTabViewModel { get; }
    public RenovationTabViewModel RenovationTabViewModel { get; }
    public RoomTabViewModel RoomTabViewModel { get; }
    public HospitalSurveyTabViewModel HospitalSurveyTabViewModel { get; } 

}