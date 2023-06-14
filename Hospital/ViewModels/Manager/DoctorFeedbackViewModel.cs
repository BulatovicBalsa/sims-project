using System.Collections.ObjectModel;
using Hospital.Charting;
using Hospital.Converters;
using Hospital.Models.Doctor;
using Hospital.Models.Feedback;
using Hospital.Repositories.Doctor;
using Hospital.Repositories.Feedback;

namespace Hospital.ViewModels.Manager;

public class DoctorFeedbackViewModel : ViewModelBase
{
    private ObservableCollection<DoctorFeedback> _allFeedback;
    private ObservableCollection<Doctor> _doctors;
    private ObservableCollection<DoctorFeedback> _selectedDoctorFeedback;
    private string _selectedDoctorId;

    public DoctorFeedbackViewModel(IRatingFrequencyPlotter ratingFrequencyPlot, ICategoryPlot averageRatingByAreaPlot)
    {
        AllFeedback = new ObservableCollection<DoctorFeedback>(DoctorFeedbackRepository.Instance.GetAll());
        Doctors = new ObservableCollection<Doctor>(DoctorRepository.Instance.GetAll());
        RatingFrequencyPlot = ratingFrequencyPlot;
        AverageRatingsByAreaPlot = averageRatingByAreaPlot;
        SelectedDoctorId = "";
        SelectedDoctorFeedback = new ObservableCollection<DoctorFeedback>();
    }

    public ObservableCollection<DoctorFeedback> AllFeedback
    {
        get => _allFeedback;
        set
        {
            if (Equals(value, _allFeedback)) return;
            _allFeedback = value;
            OnPropertyChanged(nameof(AllFeedback));
        }
    }

    public ObservableCollection<DoctorFeedback> SelectedDoctorFeedback
    {
        get => _selectedDoctorFeedback;
        set
        {
            if (Equals(value, _selectedDoctorFeedback)) return;
            _selectedDoctorFeedback = value;
            OnPropertyChanged(nameof(SelectedDoctorFeedback));
        }
    }

    public string SelectedDoctorId
    {
        get => _selectedDoctorId;
        set
        {
            if (value == _selectedDoctorId) return;
            _selectedDoctorId = value;
            SelectedDoctorFeedback =
                new ObservableCollection<DoctorFeedback>(
                    DoctorFeedbackRepository.Instance.GetByDoctorId(_selectedDoctorId));
            PlotRatingFrequencies();
            PlotAverageRatingsByArea();
            OnPropertyChanged(nameof(SelectedDoctorId));
        }
    }

    public ObservableCollection<Doctor> Doctors
    {
        get => _doctors;
        set
        {
            if (Equals(value, _doctors)) return;
            _doctors = value;
            OnPropertyChanged(nameof(Doctors));
        }
    }

    public IRatingFrequencyPlotter RatingFrequencyPlot { get; set; }
    public ICategoryPlot AverageRatingsByAreaPlot { get; set; }

    public void PlotRatingFrequencies()
    {
        var ratingFrequencies = DoctorFeedbackRepository.Instance.GetOverallRatingFrequencies(SelectedDoctorId);
        if(ratingFrequencies.Count > 0)
            RatingFrequencyPlot.PlotRatingFrequencies(ratingFrequencies);
    }

    public void PlotAverageRatingsByArea()
    {
        var dtoConverter = new AverageDoctorRatingsByAreaToDictionaryConverter(); 
        if(!string.IsNullOrEmpty(SelectedDoctorId))
            AverageRatingsByAreaPlot.Plot(dtoConverter.Convert(DoctorFeedbackRepository.Instance.GetAverageRatingsByArea(SelectedDoctorId)));
    }
}