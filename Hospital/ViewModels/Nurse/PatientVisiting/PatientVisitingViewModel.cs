using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Hospital.Models.Patient;
using Hospital.Services;
using Hospital.Views.Nurse.PatientVisiting;

namespace Hospital.ViewModels.Nurse.PatientVisiting;

public class PatientVisitingViewModel : ViewModelBase
{
    private readonly PatientService _patientService;
    private readonly List<Patient> _hospitalizedPatientsBase;
    private ObservableCollection<Patient> _hospitalizedPatients;
    private Patient? _selectedPatient;
    private string _filterName;

    public PatientVisitingViewModel()
    {
        _patientService = new PatientService();
        _hospitalizedPatientsBase = _patientService.GetAllHospitalizedPatients();
        _hospitalizedPatients = new ObservableCollection<Patient>(_hospitalizedPatientsBase);
        _selectedPatient = null;
        VisitPatientCommand = new ViewModelCommand(ExecuteVisitPatientCommand, CanExecuteVisitPatientCommand);
        _filterName = "";
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

    public string FilterName
    {
        get => _filterName;
        set
        {
            _filterName = value;
            OnPropertyChanged(nameof(FilterName));
            FilterHospitalizedPatients();
        }
    }

    private void FilterHospitalizedPatients()
    {
        var matchingPatients = _hospitalizedPatientsBase.Where(patient => (patient.FirstName + patient.LastName).ToLower().Contains(FilterName.ToLower())).ToList();
        HospitalizedPatients = new ObservableCollection<Patient>(matchingPatients);
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