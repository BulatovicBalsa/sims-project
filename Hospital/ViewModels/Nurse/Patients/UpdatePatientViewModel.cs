using Hospital.Models.Patient;
using Hospital.Repositories.Patient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Hospital.ViewModels.Nurse.Patients
{
    public class UpdatePatientViewModel : ViewModelBase
    {
        private readonly PatientRepository _patientRepository;
        private string _firstName;
        private string _height;
        private string _jmbg;
        private string _lastName;
        private string _medicalHistory;
        private string _password;
        private string _username;
        private string _weight;

        public string FirstName
        {
            get => _firstName;
            set
            {
                _firstName = value;
                OnPropertyChanged(nameof(FirstName));
            }
        }

        public string LastName
        {
            get => _lastName;
            set
            {
                _lastName = value;
                OnPropertyChanged(nameof(LastName));
            }
        }

        public string Jmbg
        {
            get => _jmbg;
            set
            {
                _jmbg = value;
                OnPropertyChanged(nameof(Jmbg));
            }
        }

        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public string Height
        {
            get => _height;
            set
            {
                _height = value;
                OnPropertyChanged(nameof(Height));
            }
        }

        public string Weight
        {
            get => _weight;
            set
            {
                _weight = value;
                OnPropertyChanged(nameof(Weight));
            }
        }

        public string MedicalHistory
        {
            get => _medicalHistory;
            set
            {
                _medicalHistory = value;
                OnPropertyChanged(nameof(MedicalHistory));
            }
        }

        public ICommand UpdatePatientCommand { get; }
        public ICommand CancelCommand { get; }

        public UpdatePatientViewModel()
        {
            _patientRepository = new PatientRepository();

            UpdatePatientCommand = new ViewModelCommand(ExecuteUpdatePatientCommand);
            CancelCommand = new ViewModelCommand(ExecuteCancelCommand);
        }

        private void ExecuteUpdatePatientCommand(object obj)
        {
            //_patientRepository.Update();

            Application.Current.Windows[1]?.Close();
        }

        private void ExecuteCancelCommand(object obj)
        {
            Application.Current.Windows[1]?.Close();
        }
    }
}
