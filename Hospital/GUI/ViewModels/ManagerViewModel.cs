using Hospital.GUI.Charting;
using Hospital.GUI.ViewModels.PatientFeedback;
using Hospital.GUI.ViewModels.PhysicalAssets;
using Hospital.GUI.ViewModels.TimeOffRequests;

namespace Hospital.GUI.ViewModels;

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
        DoctorSurveyTabViewModel =
            new DoctorSurveyTabViewModel(doctorRatingFrequencyPlot, averageDoctorRatingByAreaPlot);
        DoctorTimeOffRequestViewModel = new DoctorTimeOffRequestViewModel();
    }


    public OrderTabViewModel OrderTabViewModel { get; }
    public TransferTabViewModel TransferTabViewModel { get; }
    public RenovationTabViewModel RenovationTabViewModel { get; }
    public RoomTabViewModel RoomTabViewModel { get; }
    public HospitalSurveyTabViewModel HospitalSurveyTabViewModel { get; }
    public DoctorSurveyTabViewModel DoctorSurveyTabViewModel { get; }
    public DoctorTimeOffRequestViewModel DoctorTimeOffRequestViewModel { get; }
}