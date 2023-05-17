﻿using Hospital.Models.Doctor;
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
        private ObservableCollection<Doctor> _allDoctors;
        private ObservableCollection<Doctor> _filteredDoctors;
        private Doctor _selectedDoctor;
        private string _firstNameSearchText;
        private string _lastNameSearchText;
        private string _specializationSearhText;

        public ObservableCollection<Doctor> FilteredDoctors
        {
            get { return _filteredDoctors; }
            set
            {
                _filteredDoctors = value;
                OnPropertyChanged(nameof(FilteredDoctors));
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
            _allDoctors = new ObservableCollection<Doctor>(GetAllDoctors());
            FilteredDoctors = new ObservableCollection<Doctor>(_allDoctors);
        }
        private void FilterDoctors()
        {
            FilteredDoctors = new ObservableCollection<Doctor>(_allDoctors
                .Where(DoctorMatchesSearch);
        }
        private bool DoctorMatchesSearch(Doctor doctor) 
        {
            return doctor.FirstName.Contains(_firstNameSearchText) &&
                doctor.LastName.Contains(_lastNameSearchText) &&
                doctor.Specialization.Contains(_specializationSearhText);
        }
    }
}
