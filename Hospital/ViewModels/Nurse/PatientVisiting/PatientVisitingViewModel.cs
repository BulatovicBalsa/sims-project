using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Hospital.Models.Patient;
using Hospital.Services;
using Hospital.Views.Nurse.PatientVisiting;

namespace Hospital.ViewModels.Nurse.PatientVisiting;

public class PatientVisitingViewModel : ViewModelBase
{
    private readonly PatientService _patientService;
    private ObservableCollection<Patient> _hospitalizedPatients;
    private Patient? _selectedPatient;

    public PatientVisitingViewModel()
    {
        _patientService = new PatientService();
        _hospitalizedPatients = new ObservableCollection<Patient>(_patientService.GetAllHospitalizedPatients());
        _selectedPatient = null;
        VisitPatientCommand = new ViewModelCommand(ExecuteVisitPatientCommand, CanExecuteVisitPatientCommand);
    }

    public ObservableCollection<Patient> HospitalizedPatients
    {
        get => _hospitalizedPatients;
        set
        {
            _hospitalizedPatients = value;
            OnPropertyChanged(nameof(HospitalizedPatients));
        }
    }

    public Patient? SelectedPatient
    {
        get => _selectedPatient;
        set
        {
            _selectedPatient = value;
            OnPropertyChanged(nameof(SelectedPatient));
        }
    }

    public ICommand VisitPatientCommand { get; }

    private void ExecuteVisitPatientCommand(object obj)
    {
        var visitingDialogViewModel = new VisitingDialogViewModel(SelectedPatient!.Id);
        var visitingDialog = new VisitingDialogView()
        {
            DataContext = visitingDialogViewModel
        };

        visitingDialog.ShowDialog();
    }

    private bool CanExecuteVisitPatientCommand(object obj)
    {
        return SelectedPatient != null;
    }
}