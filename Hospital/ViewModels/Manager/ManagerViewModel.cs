using Hospital.Charting;

namespace Hospital.ViewModels.Manager;

public class ManagerViewModel : ViewModelBase
{
    public ManagerViewModel(IRatingFrequencyPlotter hospitalRatingFrequencyPlotter,
        ICategoryPlot averageHospitalFeedbackRatingByAreaPlot)
    {
        OrderTabViewModel = new OrderTabViewModel();
        TransferTabViewModel = new TransferTabViewModel();
        RenovationTabViewModel = new RenovationTabViewModel();
        RoomTabViewModel = new RoomTabViewModel();
        HospitalSurveyTabViewModel =
            new HospitalSurveyTabViewModel(hospitalRatingFrequencyPlotter, averageHospitalFeedbackRatingByAreaPlot);
        DoctorFeedbackViewModel = new DoctorFeedbackViewModel();
    }


    public OrderTabViewModel OrderTabViewModel { get; }
    public TransferTabViewModel TransferTabViewModel { get; }
    public RenovationTabViewModel RenovationTabViewModel { get; }
    public RoomTabViewModel RoomTabViewModel { get; }
    public HospitalSurveyTabViewModel HospitalSurveyTabViewModel { get; }
    public DoctorFeedbackViewModel DoctorFeedbackViewModel { get; }
}