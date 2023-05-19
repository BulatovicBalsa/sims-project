using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Hospital.Models.Patient;
using Hospital.Services;
using Microsoft.VisualBasic;

namespace Hospital.ViewModels
{
    public class CreateHospitalTreatmentReferralViewModel : ViewModelBase
    {
        private readonly PatientService _patientService = new();
        private int _duration;
        public int Duration
        {
            get => _duration;
            set
            {
                _duration = value;
                OnPropertyChanged(nameof(Duration));
            }
        }

        private ObservableCollection<string> _additionalTests = new();
        public ObservableCollection<string> AdditionalTests
        {
            get => _additionalTests;
            set
            {
                _additionalTests = value;
                OnPropertyChanged(nameof(AdditionalTests));
            }
        }

        private string? _selectedTest;
        public string? SelectedTest
        {
            get => _selectedTest;
            set
            {
                _selectedTest = value;
                OnPropertyChanged(nameof(SelectedTest));
            }
        }

        public HospitalTreatmentReferral Referral { get; set; } = new();
        private Patient _patientOnExamination;

        public ICommand AddReferralCommand { get; set; }
        public ICommand AddAdditionalTestCommand { get; set; }
        public ICommand DeleteAdditionalTestCommand { get; set; }

        public CreateHospitalTreatmentReferralViewModel(Patient patientOnExamination)
        {
            _patientOnExamination = patientOnExamination;
            Duration = 1;
            AdditionalTests = new ObservableCollection<string>();

            AddReferralCommand = new RelayCommand(AddReferral);
            AddAdditionalTestCommand = new RelayCommand(AddAdditionalTest);
            DeleteAdditionalTestCommand = new RelayCommand(DeleteAdditionalTest);
        }

        private void DeleteAdditionalTest()
        {
            if (SelectedTest is null)
            {
                MessageBox.Show("You must select additional test in order to delete it", "", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            AdditionalTests.Remove(SelectedTest);
        }

        private void AddAdditionalTest()
        {
            var testToAdd = Interaction.InputBox($"Insert additional test: ", $"Add additional test");
            AdditionalTests.Add(testToAdd);
        }

        private void AddReferral()
        {
            Referral.AdditionalTests = AdditionalTests.ToList();
            Referral.Duration = Duration;
            _patientOnExamination.HospitalTreatmentReferrals.Add(Referral);
            _patientService.UpdatePatient(_patientOnExamination);            
        }
    }
}
