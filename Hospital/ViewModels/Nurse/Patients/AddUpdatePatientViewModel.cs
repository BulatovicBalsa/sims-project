using System.Linq;
using System.Windows;
using System.Windows.Input;
using Hospital.Models.Patient;
using Hospital.Repositories.Patient;

namespace Hospital.ViewModels.Nurse.Patients;

public class AddUpdatePatientViewModel : ViewModelBase
{
    private readonly PatientRepository _patientRepository;
    private Patient _patientToUpdate;
    private string _firstName;
    private string _height;
    private string _heightError;
    private string _jmbg;
    private string _jmbgError;
    private string _lastName;
    private string _medicalHistory;
    private string _medicalHistoryError;
    private string _password;
    private string _passwordError;
    private string _username;
    private string _usernameError;
    private string _weight;
    private string _weightError;

    public AddUpdatePatientViewModel()
    {
        // dummy constructor
    }

    public AddUpdatePatientViewModel(PatientRepository patientRepository)
    {
        _patientRepository = patientRepository;

        AddPatientCommand = new ViewModelCommand(ExecuteAddPatientCommand, CanExecuteAddUpdatePatientCommand);
        CancelCommand = new ViewModelCommand(ExecuteCancelCommand);
    }

    public AddUpdatePatientViewModel(PatientRepository patientRepository, Patient selectedPatient)
    {
        _patientToUpdate = selectedPatient;
        _patientRepository = patientRepository;

        _firstName = selectedPatient.FirstName;
        _lastName = selectedPatient.LastName;
        _jmbg = selectedPatient.Jmbg;
        _username = selectedPatient.Profile.Username;
        _password = selectedPatient.Profile.Password;
        _height = selectedPatient.MedicalRecord.Height.ToString();
        _weight = selectedPatient.MedicalRecord.Weight.ToString();
        _medicalHistory = selectedPatient.MedicalRecord.GetMedicalHistoryString();

        UpdatePatientCommand = new ViewModelCommand(ExecuteUpdatePatientCommand, CanExecuteAddUpdatePatientCommand);
        CancelCommand = new ViewModelCommand(ExecuteCancelCommand);
    }

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
    public ICommand UpdatePatientCommand { get; }
    public ICommand CancelCommand { get; }

    private void ExecuteAddPatientCommand(object obj)
    {
        if (Jmbg.Length != 13)
            JmbgError = "* JMBG needs to have exactly 13 digits";
        else if (!_jmbg.All(char.IsDigit))
            JmbgError = "* JMBG can contain only digits";
        else
            JmbgError = "";

        if (Username.Length < 4)
            UsernameError = "* Username needs to have at least 4 characters";
        else
            UsernameError = "";

        if (Password.Length < 4)
            PasswordError = "* Username needs to have at least 4 characters";
        else
            PasswordError = "";

        if (!int.TryParse(Height, out _))
            HeightError = "* Height needs to be a numeric value";
        else if (int.Parse(Height) < 30 || int.Parse(Height) > 250)
            HeightError = "* Invalid height";
        else
            HeightError = "";

        if (!int.TryParse(Weight, out _))
            WeightError = "* Weight needs to be a numeric value";
        else if (int.Parse(Weight) < 1 || int.Parse(Weight) > 200)
            WeightError = "* Invalid weight";
        else
            WeightError = "";

        if (!string.IsNullOrEmpty(JmbgError) || !string.IsNullOrEmpty(UsernameError) ||
            !string.IsNullOrEmpty(PasswordError) || !string.IsNullOrEmpty(HeightError) ||
            !string.IsNullOrEmpty(WeightError))
            return;

        _patientRepository.Add(new Patient(FirstName, LastName, Jmbg, Username, Password,
            new MedicalRecord(int.Parse(Height), int.Parse(Weight))));

        Application.Current.Windows[1]?.Close();
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

    private bool CanExecuteAddUpdatePatientCommand(object obj)
    {
        var isAnyFieldNullOrEmpty = (!string.IsNullOrEmpty(FirstName) &&
                     !string.IsNullOrEmpty(LastName) &&
                     !string.IsNullOrEmpty(Jmbg) &&
                     !string.IsNullOrEmpty(Username) &&
                     !string.IsNullOrEmpty(Password) &&
                     !string.IsNullOrEmpty(Height) &&
                     !string.IsNullOrEmpty(Weight));

        return isAnyFieldNullOrEmpty;
    }

    private void ExecuteCancelCommand(object obj)
    {
        Application.Current.Windows[1]?.Close();
    }
}
