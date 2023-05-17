using Hospital.Models.Doctor;
using Hospital.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.ViewModels
{
    class DoctorSearchViewModel : ViewModelBase
    {
        private DoctorSearchService _doctorSearchService;
        private ObservableCollection<Doctor> _allDoctors;
        private ObservableCollection<Doctor> _filteredDoctors;
        private Doctor _selectedDoctor;
        private string _firstNameSearchText=string.Empty;
        private string _lastNameSearchText=string.Empty;
        private string _specializationSearhText = string.Empty;

        public ObservableCollection<Doctor> FilteredDoctors
        {
            get { return _filteredDoctors; }
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
                if(_selectedDoctor != value)
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
                FilterDoctors();
            }
        }
        public string LastNameSearchText
        {
            get => _lastNameSearchText;
            set
            {
                _lastNameSearchText = value;
                OnPropertyChanged(nameof(LastNameSearchText));
                FilterDoctors();
            }
        }
        public string SpecializationSearhText
        {
            get => _specializationSearhText; 
            set
            {
                _specializationSearhText = value;
                OnPropertyChanged(nameof(SpecializationSearhText));
                FilterDoctors();
            }
        }

        public DoctorSearchViewModel()
        {
            _doctorSearchService = new DoctorSearchService();
            _allDoctors = new ObservableCollection<Doctor>(_doctorSearchService.GetAllDoctors());
            FilteredDoctors = new ObservableCollection<Doctor>(_allDoctors);
        }
        private void FilterDoctors()
        {
            FilteredDoctors = new ObservableCollection<Doctor>(_allDoctors
                .Where(DoctorMatchesSearch));
        }
        private bool DoctorMatchesSearch(Doctor doctor) 
        {
            return doctor.FirstName.ToLower().Contains(_firstNameSearchText.ToLower()) &&
                doctor.LastName.ToLower().Contains(_lastNameSearchText.ToLower()) &&
                doctor.Specialization.ToLower().Contains(_specializationSearhText.ToLower());
        }
    }
}
