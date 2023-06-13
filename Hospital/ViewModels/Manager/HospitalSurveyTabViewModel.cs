using System.Collections.ObjectModel;
using Hospital.Models.Feedback;
using Hospital.Repositories.Feedback;

namespace Hospital.ViewModels.Manager;

public class HospitalSurveyTabViewModel : ViewModelBase
{
    private ObservableCollection<HospitalFeedback> _allHospitalFeedback;

    public HospitalSurveyTabViewModel()
    {
        AllHospitalFeedback = new ObservableCollection<HospitalFeedback>(HospitalFeedbackRepository.Instance.GetAll());
    }

    public ObservableCollection<HospitalFeedback> AllHospitalFeedback
    {
        get => _allHospitalFeedback;
        set
        {
            if (Equals(value, _allHospitalFeedback)) return;
            _allHospitalFeedback = value;
            OnPropertyChanged(nameof(AllHospitalFeedback));
        }
    }
}