using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
    private ObservableCollection<KeyValuePair<string, Dictionary<int, int>>> _selectedDoctorRatingFrequenciesByArea;
    private readonly DoctorFeedbackRepository _doctorFeedbackRepository = DoctorFeedbackRepository.Instance;
    private KeyValuePair<string, Dictionary<int, int>> _selectedAreaRatingFrequencies;

    public DoctorFeedbackViewModel(IRatingFrequencyPlot ratingFrequencyPlot, ICategoryPlot averageRatingByAreaPlot)
    {
        AllFeedback = new ObservableCollection<DoctorFeedback>(_doctorFeedbackRepository.GetAll());
        Doctors = new ObservableCollection<Doctor>(DoctorRepository.Instance.GetAll());
        SelectedDoctorRatingFrequenciesByArea = new ObservableCollection<KeyValuePair<string, Dictionary<int, int>>>();
        Top3Doctors = new ObservableCollection<Doctor>(_doctorFeedbackRepository.GetTop3Doctors()
            .Select(e => DoctorRepository.Instance.GetById(e.DoctorId)).ToList());
        Bottom3Doctors = new ObservableCollection<Doctor>(_doctorFeedbackRepository.GetBottom3Doctors()
            .Select(e => DoctorRepository.Instance.GetById(e.DoctorId)).ToList());
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
                    _doctorFeedbackRepository.GetByDoctorId(_selectedDoctorId));
            PlotRatingFrequencies();
            PlotAverageRatingsByArea();
            RefreshSelectedDoctorRatingFrequenciesByArea();
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

    public ObservableCollection<KeyValuePair<string, Dictionary<int, int>>> SelectedDoctorRatingFrequenciesByArea
    {
        get => _selectedDoctorRatingFrequenciesByArea;
        set
        {
            if (Equals(value, _selectedDoctorRatingFrequenciesByArea)) return;
            _selectedDoctorRatingFrequenciesByArea = value;
            OnPropertyChanged(nameof(SelectedDoctorRatingFrequenciesByArea));
        }
    }

    public KeyValuePair<string, Dictionary<int, int>> SelectedAreaRatingFrequencies
    {
        get => _selectedAreaRatingFrequencies;
        set
        {
            if (value.Equals(_selectedAreaRatingFrequencies)) return;
            _selectedAreaRatingFrequencies = value;
            PlotRatingFrequencies();
            OnPropertyChanged(nameof(SelectedAreaRatingFrequencies));
        }
    }

    public ObservableCollection<Doctor> Top3Doctors { get; }
    public ObservableCollection<Doctor> Bottom3Doctors { get; }

    public IRatingFrequencyPlot RatingFrequencyPlot { get; set; }
    public ICategoryPlot AverageRatingsByAreaPlot { get; set; }


    private void PlotRatingFrequencies()
    {
        var ratingFrequencies = SelectedAreaRatingFrequencies.Value;
        if(ratingFrequencies is { Count: > 0 })
            RatingFrequencyPlot.Plot(ratingFrequencies);
    }

    private void PlotAverageRatingsByArea()
    {
        var dtoConverter = new AverageDoctorRatingsByAreaToDictionaryConverter(); 
        if(!string.IsNullOrEmpty(SelectedDoctorId))
            AverageRatingsByAreaPlot.Plot(dtoConverter.Convert(_doctorFeedbackRepository.GetAverageRatingsByArea(SelectedDoctorId)));
    }

    private void RefreshSelectedDoctorRatingFrequenciesByArea()
    {
        if (string.IsNullOrEmpty(SelectedDoctorId)) return;
        SelectedDoctorRatingFrequenciesByArea =
            new ObservableCollection<KeyValuePair<string, Dictionary<int, int>>>()
            {
                new("Overall rating frequency",
                    _doctorFeedbackRepository.GetOverallRatingFrequencies(SelectedDoctorId)),
                new("Quality rating frequency", _doctorFeedbackRepository.GetDoctorQualityRatingFrequencies(SelectedDoctorId)),
                new("Recommendation rating frequency", _doctorFeedbackRepository.GetRecommendationRatingFrequencies(SelectedDoctorId))
            };
        SelectedAreaRatingFrequencies = SelectedDoctorRatingFrequenciesByArea[0];
    }
}