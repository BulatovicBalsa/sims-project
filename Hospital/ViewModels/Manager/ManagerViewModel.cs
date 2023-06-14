using Hospital.Charting;

namespace Hospital.ViewModels.Manager;

public class ManagerViewModel : ViewModelBase
{
    public ManagerViewModel(IRatingFrequencyPlot hospitalRatingFrequencyPlot,
        ICategoryPlot averageHospitalFeedbackRatingByAreaPlot, IRatingFrequencyPlot doctorRatingFrequencyPlot,
        ICategoryPlot averageDoctorRatingByAreaPlot)
    {
        OrderTabViewModel = new OrderTabViewModel();
        TransferTabViewModel = new TransferTabViewModel();
        RenovationTabViewModel = new RenovationTabViewModel();
        RoomTabViewModel = new RoomTabViewModel();
        HospitalSurveyTabViewModel =
            new HospitalSurveyTabViewModel(hospitalRatingFrequencyPlot, averageHospitalFeedbackRatingByAreaPlot);
        DoctorFeedbackViewModel = new DoctorFeedbackViewModel(doctorRatingFrequencyPlot, averageDoctorRatingByAreaPlot);
    }


    public OrderTabViewModel OrderTabViewModel { get; }
    public TransferTabViewModel TransferTabViewModel { get; }
    public RenovationTabViewModel RenovationTabViewModel { get; }
    public RoomTabViewModel RoomTabViewModel { get; }
    public HospitalSurveyTabViewModel HospitalSurveyTabViewModel { get; }
    public DoctorFeedbackViewModel DoctorFeedbackViewModel { get; }
}