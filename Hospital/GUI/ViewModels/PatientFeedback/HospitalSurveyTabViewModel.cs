using System.Collections.Generic;
using System.Collections.ObjectModel;
using Hospital.Core.PatientFeedback.Models;
using Hospital.Core.PatientFeedback.Repositories;
using Hospital.GUI.Charting;
using Hospital.GUI.Converters;

namespace Hospital.GUI.ViewModels.PatientFeedback;

public class HospitalSurveyTabViewModel : ViewModelBase
{
    private readonly ICategoryPlot _averageRatingByAreaPlot;
    private readonly IRatingFrequencyPlot _ratingFrequencyPlot;
    private ObservableCollection<HospitalFeedback> _allHospitalFeedback;
    private ObservableCollection<KeyValuePair<string, Dictionary<int, int>>> _ratingFrequenciesByArea;
    private KeyValuePair<string, Dictionary<int, int>> _selectedRatingFrequencies;

    public HospitalSurveyTabViewModel(IRatingFrequencyPlot ratingFrequencyPlot,
        ICategoryPlot averageRatingByAreaPlot)
    {
        AllHospitalFeedback = new ObservableCollection<HospitalFeedback>(HospitalFeedbackRepository.Instance.GetAll());

        var hospitalFeedbackRepository = HospitalFeedbackRepository.Instance;
        RatingFrequenciesByArea = new ObservableCollection<KeyValuePair<string, Dictionary<int, int>>>
        {
            new("Overall rating frequencies", hospitalFeedbackRepository.GetOverallRatingFrequencies()),
            new("Service quality rating frequencies",
                hospitalFeedbackRepository.GetServiceQualityRatingFrequencies()),
            new("Cleanliness rating frequencies",
                hospitalFeedbackRepository.GetCleanlinessRatingFrequencies()),
            new("Patient satisfaction rating frequencies",
                hospitalFeedbackRepository.GetPatientSatisfactionRatingFrequencies()),
            new("Would recommend to a friend rating frequencies",
                hospitalFeedbackRepository.GetRecommendationRatingFrequencies())
        };

        _ratingFrequencyPlot = ratingFrequencyPlot;
        _averageRatingByAreaPlot = averageRatingByAreaPlot;
        SelectedRatingFrequencies = RatingFrequenciesByArea[0];
        PlotAverageRatingsByArea();
    }

    public ObservableCollection<KeyValuePair<string, Dictionary<int, int>>> RatingFrequenciesByArea
    {
        get => _ratingFrequenciesByArea;
        set
        {
            if (Equals(value, _ratingFrequenciesByArea)) return;
            _ratingFrequenciesByArea = value;
            OnPropertyChanged(nameof(RatingFrequenciesByArea));
        }
    }

    public KeyValuePair<string, Dictionary<int, int>> SelectedRatingFrequencies
    {
        get => _selectedRatingFrequencies;
        set
        {
            if (value.Equals(_selectedRatingFrequencies)) return;
            _selectedRatingFrequencies = value;
            PlotRatingFrequencies();
            OnPropertyChanged(nameof(SelectedRatingFrequencies));
        }
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

    public void PlotRatingFrequencies()
    {
        if (SelectedRatingFrequencies.Value.Count > 0)
            _ratingFrequencyPlot.Plot(SelectedRatingFrequencies.Value);
    }

    public void PlotAverageRatingsByArea()
    {
        var dtoConverter = new AverageHospitalFeedbackRatingsToDictionaryConverter();
        var averageRatingByArea = dtoConverter.Convert(HospitalFeedbackRepository.Instance.GetAverageRatings());
        _averageRatingByAreaPlot.Plot(averageRatingByArea);
    }
}