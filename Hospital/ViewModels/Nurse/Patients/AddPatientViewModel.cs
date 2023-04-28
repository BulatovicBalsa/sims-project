using System;
using System.Windows;
using System.Windows.Input;
using Hospital.Models.Patient;
using Hospital.Repositories.Patient;

namespace Hospital.ViewModels.Nurse.Patients;

public class AddPatientViewModel : ViewModelBase
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

    private string _firstNameError;
    private string _lastNameError;
    private string _jmbgError;
    private string _usernameError;
    private string _passwordError;
    private string _heightError;
    private string _weightError;
    private string _medicalHistoryError;

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

    public string FirstNameError    
    {
        get => _firstNameError;
        set
        {
            _firstNameError = value;
            OnPropertyChanged(nameof(FirstNameError));
        }
    }

    public string LastNameError
    {
        get => _lastNameError;
        set
        {
            _lastNameError = value;
            OnPropertyChanged(nameof(LastNameError));
        }
    }

    public string JmbgError
    {
        get => _jmbgError;
        set
        {
            _jmbgError = value;
            OnPropertyChanged(nameof(JmbgError));
        }
    }

    public string UsernameError
    {
        get => _usernameError;
        set
        {
            _usernameError = value;
            OnPropertyChanged(nameof(UsernameError));
        }
    }

    public string PasswordError
    {
        get => _passwordError;
        set
        {
            _passwordError = value;
            OnPropertyChanged(nameof(PasswordError));
        }
    }

    public string HeightError
    {
        get => _heightError;
        set
        {
            _heightError = value;
            OnPropertyChanged(nameof(HeightError));
        }
    }

    public string WeightError
    {
        get => _weightError;
        set
        {
            _weightError = value;
            OnPropertyChanged(nameof(WeightError));
        }
    }

    public string MedicalHistoryError
    {
        get => _medicalHistoryError;
        set
        {
            _medicalHistoryError = value;
            OnPropertyChanged(nameof(MedicalHistoryError));
        }
    }

    public ICommand AddPatientCommand { get; }
    public ICommand CancelCommand { get; }

    public AddPatientViewModel()
    {
        // dummy constructor
    }

    public AddPatientViewModel(PatientRepository patientRepository)
    {
        _patientRepository = patientRepository;

        AddPatientCommand = new ViewModelCommand(ExecuteAddPatientCommand, CanExecuteAddPatientCommand);
        CancelCommand = new ViewModelCommand(ExecuteCancelCommand);
    }

    private void ExecuteAddPatientCommand(object obj)
    {
        _patientRepository.Add(new Patient(_firstName, _lastName, _jmbg, _username, _password, new MedicalRecord(int.Parse(_height), int.Parse(_weight))));
       
        Application.Current.Windows[1]?.Close();
    }

    private bool CanExecuteAddPatientCommand(object obj)
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
