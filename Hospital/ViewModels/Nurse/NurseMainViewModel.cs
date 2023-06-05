﻿using System.Windows.Input;
using Hospital.ViewModels.Nurse.Medication;
using Hospital.ViewModels.Nurse.PatientAdmission;
using Hospital.ViewModels.Nurse.Patients;
using Hospital.ViewModels.Nurse.Referrals;
using Hospital.ViewModels.Nurse.UrgentExaminations;

namespace Hospital.ViewModels.Nurse;

public class NurseMainViewModel : ViewModelBase
{
    private ViewModelBase _currentChildView;
    public NurseMainViewModel()
    {
        ShowPatientsViewCommand = new ViewModelCommand(ExecuteShowPatientsViewCommand);
        ShowPatientAdmissionViewCommand = new ViewModelCommand(ExecuteShowPatientAdmissionViewCommand);
        ShowUrgentExaminationsViewCommand = new ViewModelCommand(ExecuteShowUrgentExaminationsViewCommand);
        ShowPatientReferralsViewCommand = new ViewModelCommand(ExecuteShowPatientReferralsViewCommand);
        ShowMedicationManagementViewCommand = new ViewModelCommand(ExecuteShowMedicationManagementViewCommand);
        ShowCommunicationViewCommand = new ViewModelCommand(ExecuteShowCommunicationViewCommand);

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
        //CurrentChildView = new CommunicationViewModel();
    }
}
