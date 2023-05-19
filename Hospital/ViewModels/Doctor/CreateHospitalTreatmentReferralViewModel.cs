using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Hospital.Models.Patient;

namespace Hospital.ViewModels
{
    public class CreateHospitalTreatmentReferralViewModel : ViewModelBase
    {
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

        private string? _selectedAdditionalTest;
        public string? SelectedAdditionalTest
        {
            get => _selectedAdditionalTest;
            set
            {
                _selectedAdditionalTest = value;
                OnPropertyChanged(nameof(SelectedAdditionalTest));
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
            throw new NotImplementedException();
        }

        private void AddAdditionalTest()
        {
            throw new NotImplementedException();
        }

        private void AddReferral()
        {
            throw new NotImplementedException();
        }
    }
}
