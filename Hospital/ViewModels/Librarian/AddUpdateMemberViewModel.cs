using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Hospital.Models;
using Hospital.Models.Memberships;
using Hospital.Repositories;
using Hospital.Repositories.Memberships;
using Hospital.Serialization;

namespace Hospital.ViewModels.Librarian;

public class AddUpdateMemberViewModel : ViewModelBase
{
    private readonly MemberRepository _memberRepository;
    private readonly MembershipRepository _membershipRepository;
    private readonly Member? _memberToUpdate;
    private string _firstName;
    private string _lastName;
    private string _jmbg;
    private string _jmbgError;
    private string _password;
    private string _passwordError;
    private string _username;
    private string _usernameError;
    private string _email;
    private string _emailError;
    private string _birthDate;
    private string _birthDateError;
    private string _phoneNumber;
    private string _phoneNumberError;
    private string _membershipNumber;
    private string _membershipNumberError;
    private ObservableCollection<Membership> _allMemberships;
    private Membership? _selectedMembership;

    public event Action? DialogClosed;

    public AddUpdateMemberViewModel()
    {
        // dummy constructor
    }

    public AddUpdateMemberViewModel(MemberRepository memberRepository)
    {
        _memberRepository = memberRepository;
        _membershipRepository = new MembershipRepository(new CsvSerializer<Membership>());
        _allMemberships = new ObservableCollection<Membership>(_membershipRepository.GetAll());

        AddMemberCommand = new ViewModelCommand(ExecuteAddMemberCommand, CanExecuteAddUpdateMemberCommand);
        CancelCommand = new ViewModelCommand(ExecuteCancelCommand);
    }

    public AddUpdateMemberViewModel(MemberRepository memberRepository, Member selectedMember)
    {
        _memberToUpdate = selectedMember;
        _memberRepository = memberRepository;
        _membershipRepository = new MembershipRepository(new CsvSerializer<Membership>());
        _allMemberships = new ObservableCollection<Membership>(_membershipRepository.GetAll());

        SetViewModelProperties(selectedMember);

        UpdateMemberCommand = new ViewModelCommand(ExecuteUpdateMemberCommand, CanExecuteAddUpdateMemberCommand);
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

    public string Email
    {
        get => _email;
        set
        {
            _email = value;
            OnPropertyChanged(nameof(Email));
        }
    }

    public string EmailError
    {
        get => _emailError;
        set
        {
            _emailError = value;
            OnPropertyChanged(nameof(EmailError));
        }
    }

    public string BirthDate
    {
        get => _birthDate;
        set
        {
            _birthDate = value;
            OnPropertyChanged(nameof(BirthDate));
        }
    }

    public string BirthDateError
    {
        get => _birthDateError;
        set
        {
            _birthDateError = value;
            OnPropertyChanged(nameof(BirthDateError));
        }
    }

    public string PhoneNumber
    {
        get => _phoneNumber;
        set
        {
            _phoneNumber = value;
            OnPropertyChanged(nameof(PhoneNumber));
        }
    }

    public string PhoneNumberError
    {
        get => _phoneNumberError;
        set
        {
            _phoneNumberError = value;
            OnPropertyChanged(nameof(PhoneNumberError));
        }
    }

    public string MembershipNumber
    {
        get => _membershipNumber;
        set
        {
            _membershipNumber = value;
            OnPropertyChanged(nameof(MembershipNumber));
        }
    }

    public string MembershipNumberError
    {
        get => _membershipNumberError;
        set
        {
            _membershipNumberError = value;
            OnPropertyChanged(nameof(MembershipNumberError));
        }
    }

    public ObservableCollection<Membership> AllMemberships
    {
        get => _allMemberships;
        set
        {
            _allMemberships = value;
            OnPropertyChanged(nameof(AllMemberships));
        }
    }

    public Membership? SelectedMembership
    {
        get => _selectedMembership;
        set
        {
            _selectedMembership = value;
            OnPropertyChanged(nameof(SelectedMembership));
        }
    }

    public ICommand AddMemberCommand { get; }
    public ICommand UpdateMemberCommand { get; }
    public ICommand CancelCommand { get; }

    private void SetViewModelProperties(Member selectedMember)
    {
        FirstName = selectedMember.FirstName;
        LastName = selectedMember.LastName;
        Jmbg = selectedMember.JMBG;
        Username = selectedMember.Profile.Username;
        Password = selectedMember.Profile.Password;
        Email = selectedMember.Email;
        PhoneNumber = selectedMember.PhoneNumber;
        MembershipNumber = selectedMember.MembershipNumber;
        BirthDate = selectedMember.BirthDate.ToString("dd/MM/yyyy");
        SelectedMembership = selectedMember.Membership;
    }

    private void ExecuteAddMemberCommand(object obj)
    {
        CheckInputErrors();

        if (ErrorHappened())
            return;

        _memberRepository.Add(new Member(FirstName, LastName, DateTime.Parse(BirthDate), Email, PhoneNumber,
            Jmbg, MembershipNumber, DateTime.Now.AddYears(1), new Membership(), Username, Password));

        CloseDialog();
    }

    private void ExecuteUpdateMemberCommand(object obj)
    {
        CheckInputErrors();

        if (ErrorHappened())
            return;

        SetMemberFromProperties();

        _memberRepository.Update(_memberToUpdate);

        CloseDialog();
    }

    private void SetMemberFromProperties()
    {
        _memberToUpdate.FirstName = FirstName;
        _memberToUpdate.LastName = LastName;
        _memberToUpdate.JMBG = Jmbg;
        _memberToUpdate.Profile.Username = Username;
        _memberToUpdate.Profile.Password = Password;
        _memberToUpdate.Email = Email;
        _memberToUpdate.PhoneNumber = PhoneNumber;
        _memberToUpdate.MembershipNumber = MembershipNumber;
        _memberToUpdate.BirthDate = DateTime.Parse(BirthDate);
        _memberToUpdate.Membership = SelectedMembership;
    }

    private void CloseDialog()
    {
        Application.Current.Windows[1]?.Close();
        DialogClosed?.Invoke();
    }

    private bool ErrorHappened()
    {
        return !string.IsNullOrEmpty(JmbgError) || !string.IsNullOrEmpty(UsernameError) ||
               !string.IsNullOrEmpty(PasswordError) || !string.IsNullOrEmpty(EmailError) ||
               !string.IsNullOrEmpty(BirthDateError) || !string.IsNullOrEmpty(PhoneNumberError) ||
               !string.IsNullOrEmpty(MembershipNumberError);
    }

    private void CheckInputErrors()
    {
        CheckJmbgErrors();
        CheckUsernameErrors();
        CheckPasswordErrors();
        CheckEmailErrors();
        CheckBirthDateErrors();
        CheckPhoneNumberErrors();
        CheckMembershipNumberErrors();
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
        if (_memberToUpdate != null && Username == _memberToUpdate.Profile.Username)
            return false;

        return _memberRepository.GetByUsername(Username) != null;
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

    private bool IsValidEmail(string email)
    {
        var trimmedEmail = email.Trim();

        if (trimmedEmail.EndsWith("."))
        {
            return false; // suggested by @TK-421
        }
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == trimmedEmail;
        }
        catch
        {
            return false;
        }
    }

    private void CheckEmailErrors()
    {
        if (!IsValidEmail(Email))
        {
            EmailError = "Email is invalid.";
        }
    }

    private void CheckBirthDateErrors()
    {
        if (!DateTime.TryParse(BirthDate, out _))
        {
            BirthDateError = "Invalid date.";
        }
    }

    private void CheckPhoneNumberErrors()
    {
        if (!PhoneNumber.All(char.IsDigit))
        {
            PhoneNumberError = "Invalid phone number.";
        }
    }

    private void CheckMembershipNumberErrors()
    {
        return;
    }

    private bool CanExecuteAddUpdateMemberCommand(object obj)
    {
        var isAnyFieldNullOrEmpty = !string.IsNullOrEmpty(FirstName) &&
                                    !string.IsNullOrEmpty(LastName) &&
                                    !string.IsNullOrEmpty(Jmbg) &&
                                    !string.IsNullOrEmpty(Username) &&
                                    !string.IsNullOrEmpty(Password) &&
                                    !string.IsNullOrEmpty(Email) &&
                                    !string.IsNullOrEmpty(BirthDate) &&
                                    !string.IsNullOrEmpty(PhoneNumber) &&
                                    !string.IsNullOrEmpty(MembershipNumber) &&
                                    SelectedMembership != null;

        return isAnyFieldNullOrEmpty;
    }

    private void ExecuteCancelCommand(object obj)
    {
        CloseDialog();
    }
}
