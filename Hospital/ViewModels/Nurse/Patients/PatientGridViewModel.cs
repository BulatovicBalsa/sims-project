using System.Collections.ObjectModel;
using System.Windows.Input;
using Hospital.Models;
using Hospital.Models.Patient;
using Hospital.Repositories;
using Hospital.Repositories.Patient;
using Hospital.Views.Librarian.Patients;

namespace Hospital.ViewModels.Librarian.Patients;

public class PatientGridViewModel : ViewModelBase
{
    private readonly MemberRepository _memberRepository;
    private ObservableCollection<Member> _members;
    private Member? _selectedMember;

    public PatientGridViewModel()
    {
        _memberRepository = new MemberRepository();
        _members = new ObservableCollection<Member>(_memberRepository.GetAll());

        _memberRepository.MemberAdded += member =>
        {
            _members.Add(member);
        };

        _memberRepository.MemberUpdated += member =>
        {
            _members.Remove(member);
            _members.Add(member);
        };

        AddMemberCommand = new ViewModelCommand(ExecuteAddMemberCommand);
        UpdateMemberCommand = new ViewModelCommand(ExecuteUpdateMemberCommand, IsMemberSelected);
        DeleteMemberCommand = new ViewModelCommand(ExecuteDeleteMemberCommand, IsMemberSelected);
    }

    public Member? SelectedMember
    {
        get => _selectedMember;
        set
        {
            _selectedMember = value;
            OnPropertyChanged(nameof(SelectedMember));
        }
    }

    public ObservableCollection<Member> Members
    {
        get => _members;
        set
        {
            _members = value;
            OnPropertyChanged(nameof(Members));
        }
    }

    public ICommand AddMemberCommand { get; }
    public ICommand UpdateMemberCommand { get; }
    public ICommand DeleteMemberCommand { get; }
    private void ExecuteAddMemberCommand (object obj)
    {
        var addPatientDialog = new AddPatientView
        {
            DataContext = new AddUpdatePatientViewModel(_memberRepository)
        };

        addPatientDialog.ShowDialog();
    }

    private void ExecuteUpdateMemberCommand(object obj)
    {
        var updatePatientDialog = new UpdatePatientView
        {
            DataContext = new AddUpdatePatientViewModel(_memberRepository, SelectedMember)
        };

        updatePatientDialog.ShowDialog();
    }

    private void ExecuteDeleteMemberCommand(object obj)
    {
        _memberRepository.Delete(SelectedMember);
        _members.Remove(SelectedMember);
        SelectedMember = null;
    }

    private bool IsMemberSelected(object obj)
    {
        return _selectedMember != null;
    }
}