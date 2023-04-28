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

    public ICommand AddPatientCommand { get; }
    public ICommand CancelCommand { get; }

    public AddPatientViewModel()
    {
        _patientRepository = new PatientRepository();

        AddPatientCommand = new ViewModelCommand(ExecuteAddPatientCommand);
        CancelCommand = new ViewModelCommand(ExecuteCancelCommand);
    }

    private void ExecuteAddPatientCommand(object obj)
    {
        _patientRepository.Add(new Patient(_firstName, _lastName, _jmbg, _username, _password, new MedicalRecord(int.Parse(_height), int.Parse(_weight))));
       
        Application.Current.Windows[1]?.Close();
    }

    private void ExecuteCancelCommand(object obj)
    {
        Application.Current.Windows[1]?.Close();
    }
}
