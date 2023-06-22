using System.Threading;
using System.Windows.Input;
using Hospital.DTOs;
using Hospital.Services;
using Hospital.ViewModels.Librarian.Medication;
using Hospital.ViewModels.Librarian.PatientAccommodation;
using Hospital.ViewModels.Librarian.PatientAdmission;
using Hospital.ViewModels.Librarian.Patients;
using Hospital.ViewModels.Librarian.PatientVisiting;
using Hospital.ViewModels.Librarian.Referrals;
using Hospital.ViewModels.Librarian.UrgentExaminations;
using Hospital.Views;

namespace Hospital.ViewModels.Librarian;

public class LibrarianMainViewModel : ViewModelBase
{
    private ViewModelBase _currentChildView;
    private LibrarianService _librarianService = new();
    public LibrarianMainViewModel()
    {
        ShowPatientsViewCommand = new ViewModelCommand(ExecuteShowPatientsViewCommand);
        ShowPatientAdmissionViewCommand = new ViewModelCommand(ExecuteShowPatientAdmissionViewCommand);
        ShowUrgentExaminationsViewCommand = new ViewModelCommand(ExecuteShowUrgentExaminationsViewCommand);
        ShowPatientReferralsViewCommand = new ViewModelCommand(ExecuteShowPatientReferralsViewCommand);
        ShowMedicationManagementViewCommand = new ViewModelCommand(ExecuteShowMedicationManagementViewCommand);
        ShowCommunicationViewCommand = new ViewModelCommand(ExecuteShowCommunicationViewCommand);
        ShowPatientAccommodationViewCommand = new ViewModelCommand(ExecuteShowPatientAccommodationViewCommand);
        ShowPatientVisitingViewCommand = new ViewModelCommand(ExecuteShowPatientVisitingViewCommand);

        ExecuteShowPatientsViewCommand(null);
    }

    public ViewModelBase CurrentChildView
    {
        get => _currentChildView;
        set
        {
            _currentChildView = value;
            OnPropertyChanged(nameof(CurrentChildView));
        }
    }
    public ICommand ShowPatientsViewCommand { get; }
    public ICommand ShowPatientAdmissionViewCommand { get; }
    public ICommand ShowUrgentExaminationsViewCommand { get; }
    public ICommand ShowPatientReferralsViewCommand { get; }
    public ICommand ShowMedicationManagementViewCommand { get; }
    public ICommand ShowCommunicationViewCommand { get; }
    public ICommand ShowPatientAccommodationViewCommand { get; }
    public ICommand ShowPatientVisitingViewCommand { get; }

    private void ExecuteShowPatientsViewCommand(object? obj)
    {
        CurrentChildView = new PatientGridViewModel();
    }

    private void ExecuteShowPatientAdmissionViewCommand(object obj)
    {
        CurrentChildView = new PatientAdmissionViewModel();
    }

    private void ExecuteShowUrgentExaminationsViewCommand(object obj)
    {
        CurrentChildView = new UrgentExaminationsViewModel();
    }

    private void ExecuteShowPatientReferralsViewCommand(object obj)
    {
        CurrentChildView = new PatientReferralsViewModel();
    }

    private void ExecuteShowMedicationManagementViewCommand(object obj)
    {
        CurrentChildView = new MedicationManagementViewModel();
    }
    private void ExecuteShowCommunicationViewCommand(object obj)
    {
        PersonDTO loggedUser = _librarianService.GetLoggedInLibrarian();
        CommunicationView communicationView = new CommunicationView(loggedUser);
        communicationView.Show();
    }

    private void ExecuteShowPatientAccommodationViewCommand(object obj)
    {
        CurrentChildView = new PatientAccommodationViewModel();
    }

    private void ExecuteShowPatientVisitingViewCommand(object obj)
    {
        CurrentChildView = new PatientVisitingViewModel();
    }
}
