using System.Collections.ObjectModel;
using Hospital.DoctorSearch.Services;
using Hospital.Workers.Models;

namespace Hospital.GUI.ViewModels.DoctorSearch;

internal class DoctorSearchViewModel : ViewModelBase
{
    private readonly ObservableCollection<Doctor> _allDoctors;
    private readonly DoctorSearchService _doctorSearchService;
    private ObservableCollection<Doctor> _filteredDoctors;
    private string _firstNameSearchText = string.Empty;
    private string _lastNameSearchText = string.Empty;
    private Doctor _selectedDoctor;
    private string _specializationSearhText = string.Empty;

    public DoctorSearchViewModel()
    {
        _doctorSearchService = new DoctorSearchService();
        _allDoctors = new ObservableCollection<Doctor>(_doctorSearchService.GetAllDoctors());
        FilteredDoctors = new ObservableCollection<Doctor>(_allDoctors);
    }

    public ObservableCollection<Doctor> FilteredDoctors
    {
        get => _filteredDoctors;
        set
        {
            _filteredDoctors = value;
            OnPropertyChanged(nameof(FilteredDoctors));
        }
    }

    public Doctor SelectedDoctor
    {
        get => _selectedDoctor;
        set
        {
            if (_selectedDoctor != value)
            {
                _selectedDoctor = value;
                OnPropertyChanged(nameof(SelectedDoctor));
            }
        }
    }

    public string FirstNameSearchText
    {
        get => _firstNameSearchText;
        set
        {
            _firstNameSearchText = value;
            OnPropertyChanged(nameof(FirstNameSearchText));
            UpdateFilteredDoctors();
        }
    }

    public string LastNameSearchText
    {
        get => _lastNameSearchText;
        set
        {
            _lastNameSearchText = value;
            OnPropertyChanged(nameof(LastNameSearchText));
            UpdateFilteredDoctors();
        }
    }

    public string SpecializationSearhText
    {
        get => _specializationSearhText;
        set
        {
            _specializationSearhText = value;
            OnPropertyChanged(nameof(SpecializationSearhText));
            UpdateFilteredDoctors();
        }
    }

    private void UpdateFilteredDoctors()
    {
        _doctorSearchService.FilterDoctors(FirstNameSearchText, LastNameSearchText, SpecializationSearhText);
        FilteredDoctors = new ObservableCollection<Doctor>(_doctorSearchService.GetFilteredDoctors());
    }
}