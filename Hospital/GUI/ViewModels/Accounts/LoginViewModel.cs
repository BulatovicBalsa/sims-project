using System.Net;
using System.Security;
using System.Security.Principal;
using System.Threading;
using System.Windows.Input;
using Hospital.Core.Accounts.Services;
using Hospital.Core.PatientHealthcare.Models;
using Hospital.Core.Workers.Models;
using Hospital.Exceptions;

namespace Hospital.GUI.ViewModels.Accounts;

public class LoginViewModel : ViewModelBase
{
    private readonly LoginService _loginService;
    private string _errorMessage;
    private bool _isViewVisible = true;
    private SecureString _password;
    private string _username;

    public LoginViewModel()
    {
        LoginCommand = new ViewModelCommand(ExecuteLoginCommand, CanExecuteLoginCommand);
        _password = new SecureString();
        _loginService = new LoginService();
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

    public SecureString Password
    {
        get => _password;
        set
        {
            _password = value;
            OnPropertyChanged(nameof(Password));
        }
    }

    public string ErrorMessage
    {
        get => _errorMessage;
        set
        {
            _errorMessage = value;
            OnPropertyChanged(nameof(ErrorMessage));
        }
    }

    public bool IsViewVisible
    {
        get => _isViewVisible;
        set
        {
            _isViewVisible = value;
            OnPropertyChanged(nameof(IsViewVisible));
        }
    }

    public ICommand LoginCommand { get; }

    private bool CanExecuteLoginCommand(object obj)
    {
        var emptyOrShortFields = string.IsNullOrWhiteSpace(Username) || Username.Length < 4 ||
                                 Password.Length < 4 || string.IsNullOrWhiteSpace(Password?.ToString());

        return !emptyOrShortFields;
    }

    private void ExecuteLoginCommand(object obj)
    {
        if (_loginService.AuthenticateUser(new NetworkCredential(Username, Password)))
        {
            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(GetIdentity()), null);
            IsViewVisible = false;
        }
        else
        {
            _loginService.LoggedUser = null;
            ErrorMessage = "* Invalid username or password";
        }
    }

    private string GetIdentity()
    {
        if (_loginService.LoggedUser == null) return "";

        var userId = _loginService.LoggedUser.Id;
        return $"{userId}|{GetUserType()}";
    }

    private string GetUserType()
    {
        if (_loginService.LoggedUser?.GetType() == typeof(Patient)) return "PATIENT";
        if (_loginService.LoggedUser?.GetType() == typeof(Doctor)) return "DOCTOR";
        if (_loginService.LoggedUser?.GetType() == typeof(Nurse)) return "NURSE";
        if (_loginService.LoggedUser?.GetType() == typeof(Manager)) return "MANAGER";

        throw new UnrecognizedUserTypeException();
    }
}