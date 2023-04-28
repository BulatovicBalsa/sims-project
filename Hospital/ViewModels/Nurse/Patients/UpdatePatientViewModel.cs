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
        private Patient _patientToUpdate;
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
            // dummy constructor
        }
        public UpdatePatientViewModel(PatientRepository patientRepository, Patient selectedPatient)
        {
            _firstName = selectedPatient.FirstName;
            _lastName = selectedPatient.LastName;
            _jmbg = selectedPatient.Jmbg;
            _username = selectedPatient.Profile.Username;
            _password = selectedPatient.Profile.Password;
            _height = selectedPatient.MedicalRecord.Height.ToString();
            _weight = selectedPatient.MedicalRecord.Weight.ToString();
            _medicalHistory = selectedPatient.MedicalRecord.GetMedicalHistoryString();

            _patientToUpdate = selectedPatient;
            _patientRepository = patientRepository;

            UpdatePatientCommand = new ViewModelCommand(ExecuteUpdatePatientCommand, CanExecuteUpdatePatientCommand);
            CancelCommand = new ViewModelCommand(ExecuteCancelCommand);
        }

        private void ExecuteUpdatePatientCommand(object obj)
        {
            _patientToUpdate.FirstName = _firstName;
            _patientToUpdate.LastName = _lastName;
            _patientToUpdate.Jmbg = _jmbg;
            _patientToUpdate.Profile.Username = _username;
            _patientToUpdate.Profile.Password = _password;
            _patientToUpdate.MedicalRecord.Height = int.Parse(_height);
            _patientToUpdate.MedicalRecord.Weight = int.Parse(_weight);
            _patientToUpdate.MedicalRecord.MedicalHistory = _medicalHistory.Split(", ").ToList();

            _patientRepository.Update(_patientToUpdate);

            Application.Current.Windows[1]?.Close();
        }

        private bool CanExecuteUpdatePatientCommand(object obj)
        {
            return !string.IsNullOrEmpty(_firstName) &&
                   !string.IsNullOrEmpty(_lastName) &&
                   !string.IsNullOrEmpty(_jmbg) &&
                   !string.IsNullOrEmpty(_username) &&
                   !string.IsNullOrEmpty(_password) &&
                   !string.IsNullOrEmpty(_height) &&
                   !string.IsNullOrEmpty(_weight) &&
                   !string.IsNullOrEmpty(_medicalHistory);
        }

        private void ExecuteCancelCommand(object obj)
        {
            Application.Current.Windows[1]?.Close();
        }
    }
}
