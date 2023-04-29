using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Hospital.Models.Patient;
using Hospital.Repositories.Patient;

namespace Hospital.ViewModels.Nurse.Patients;

public class AddUpdatePatientViewModel : ViewModelBase
{
    private readonly PatientRepository _patientRepository;
    private readonly Patient? _patientToUpdate;
    private string _allergies = "";
    private string _firstName;
    private string _height;
    private string _heightError;
    private string _jmbg;
    private string _jmbgError;
    private string _lastName;
    private string _medicalHistory = "";
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

        SetViewModelProperties(selectedPatient);

        UpdatePatientCommand = new ViewModelCommand(ExecuteUpdatePatientCommand, CanExecuteAddUpdatePatientCommand);
        CancelCommand = new ViewModelCommand(ExecuteCancelCommand);
    }

    public string Allergies
    {
        get => _allergies;
        set
        {
            _allergies = value;
            OnPropertyChanged(nameof(Allergies));
        }
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

    public ICommand AddPatientCommand { get; }
    public ICommand UpdatePatientCommand { get; }
    public ICommand CancelCommand { get; }

    private void SetViewModelProperties(Patient selectedPatient)
    {
        FirstName = selectedPatient.FirstName;
        LastName = selectedPatient.LastName;
        Jmbg = selectedPatient.Jmbg;
        Username = selectedPatient.Profile.Username;
        Password = selectedPatient.Profile.Password;
        Height = selectedPatient.MedicalRecord.Height.ToString();
        Weight = selectedPatient.MedicalRecord.Weight.ToString();
        MedicalHistory = GetCommaSeparatedString(selectedPatient.MedicalRecord.MedicalHistory);
        Allergies = GetCommaSeparatedString(selectedPatient.MedicalRecord.Allergies);
    }

    private string GetCommaSeparatedString(IEnumerable<string> words)
    {
        return string.Join(", ", words);
    }

    private void ExecuteAddPatientCommand(object obj)
    {
        CheckInputErrors();

        if (ErrorHappened())
            return;

        _patientRepository.Add(new Patient(FirstName, LastName, Jmbg, Username, Password,
            new MedicalRecord(int.Parse(Height), int.Parse(Weight), Allergies.Split(", ").ToList(),
                MedicalHistory.Split(", ").ToList())));

        CloseDialog();
    }

    private void ExecuteUpdatePatientCommand(object obj)
    {
        CheckInputErrors();

        if (ErrorHappened())
            return;

        SetPatientFromProperties();

        _patientRepository.Update(_patientToUpdate);

        CloseDialog();
    }

    private void SetPatientFromProperties()
    {
        _patientToUpdate.FirstName = FirstName;
        _patientToUpdate.LastName = LastName;
        _patientToUpdate.Jmbg = Jmbg;
        _patientToUpdate.Profile.Username = Username;
        _patientToUpdate.Profile.Password = Password;
        _patientToUpdate.MedicalRecord.Height = int.Parse(Height);
        _patientToUpdate.MedicalRecord.Weight = int.Parse(Weight);
        _patientToUpdate.MedicalRecord.MedicalHistory = MedicalHistory.Split(", ").ToList();
        _patientToUpdate.MedicalRecord.Allergies = Allergies.Split(", ").ToList();
    }

    private void CloseDialog()
    {
        Application.Current.Windows[1]?.Close();
    }

    private bool ErrorHappened()
    {
        return !string.IsNullOrEmpty(JmbgError) || !string.IsNullOrEmpty(UsernameError) ||
               !string.IsNullOrEmpty(PasswordError) || !string.IsNullOrEmpty(HeightError) ||
               !string.IsNullOrEmpty(WeightError);
    }

    private void CheckInputErrors()
    {
        CheckJmbgErrors();
        CheckUsernameErrors();
        CheckPasswordErrors();
        CheckHeightErrors();
        CheckWeightErrors();
    }

    private void CheckWeightErrors()
    {
        if (!int.TryParse(Weight, out _))
            WeightError = "* Weight needs to be a numeric value";
        else if (int.Parse(Weight) < 1 || int.Parse(Weight) > 200)
            WeightError = "* Invalid weight";
        else
            WeightError = "";
    }

    private void CheckHeightErrors()
    {
        if (!int.TryParse(Height, out _))
            HeightError = "* Height needs to be a numeric value";
        else if (int.Parse(Height) < 30 || int.Parse(Height) > 250)
            HeightError = "* Invalid height";
        else
            HeightError = "";
    }

    private void CheckPasswordErrors()
    {
        if (Password.Length < 4)
            PasswordError = "* Username needs to have at least 4 characters";
        else
            PasswordError = "";
    }

    private void CheckUsernameErrors()
    {
        if (Username.Length < 4)
            UsernameError = "* Username needs to have at least 4 characters";
        else if (IsUsernameTaken())
            UsernameError = "* Username already taken";
        else
            UsernameError = "";
    }

    private bool IsUsernameTaken()
    {
        if (_patientToUpdate != null && Username == _patientToUpdate.Profile.Username)
            return false;

        return _patientRepository.GetByUsername(Username) != null;
    }

    private void CheckJmbgErrors()
    {
        if (Jmbg.Length != 13)
            JmbgError = "* JMBG needs to have exactly 13 digits";
        else if (!_jmbg.All(char.IsDigit))
            JmbgError = "* JMBG can contain only digits";
        else
            JmbgError = "";
    }

    private bool CanExecuteAddUpdatePatientCommand(object obj)
    {
        var isAnyFieldNullOrEmpty = !string.IsNullOrEmpty(FirstName) &&
                                    !string.IsNullOrEmpty(LastName) &&
                                    !string.IsNullOrEmpty(Jmbg) &&
                                    !string.IsNullOrEmpty(Username) &&
                                    !string.IsNullOrEmpty(Password) &&
                                    !string.IsNullOrEmpty(Height) &&
                                    !string.IsNullOrEmpty(Weight);

        return isAnyFieldNullOrEmpty;
    }

    private void ExecuteCancelCommand(object obj)
    {
        CloseDialog();
    }
}
