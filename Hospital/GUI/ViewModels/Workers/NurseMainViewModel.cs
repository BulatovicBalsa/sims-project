using System.Windows.Input;
using Hospital.Core.Workers.Services;
using Hospital.GUI.ViewModels.PatientHealthcare;
using Hospital.GUI.ViewModels.PatientManagement;
using Hospital.GUI.ViewModels.Pharmacy;
using Hospital.GUI.ViewModels.Scheduling;
using Hospital.GUI.Views.Messaging;

namespace Hospital.GUI.ViewModels.Workers;

public class NurseMainViewModel : ViewModelBase
{
    private ViewModelBase _currentChildView;
    private readonly NurseService _nurseService = new();

    public NurseMainViewModel()
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
        var loggedUser = _nurseService.GetLoggedInNurse();
        var communicationView = new CommunicationView(loggedUser);
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