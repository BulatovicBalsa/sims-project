﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Hospital.Charting;
using Hospital.Models.Feedback;
using Hospital.Repositories.Feedback;

namespace Hospital.ViewModels.Manager;

public class HospitalSurveyTabViewModel : ViewModelBase
{
    private readonly IRatingFrequencyPlotter _ratingFrequencyPlotter;
    private ObservableCollection<HospitalFeedback> _allHospitalFeedback;
    private ObservableCollection<KeyValuePair<string, Dictionary<int, int>>> _ratingFrequenciesByArea;
    private KeyValuePair<string, Dictionary<int, int>> _selectedRatingFrequencies;

    public HospitalSurveyTabViewModel(IRatingFrequencyPlotter ratingFrequencyPlotter)
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

        _ratingFrequencyPlotter = ratingFrequencyPlotter;
        SelectedRatingFrequencies = RatingFrequenciesByArea[0];
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
        _ratingFrequencyPlotter.PlotRatingFrequencies(SelectedRatingFrequencies.Value);
    }
}