using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Hospital.Models.Patient;
using Hospital.Services;

namespace Hospital.ViewModels.Nurse.Medication;
public class MedicationManagementViewModel : ViewModelBase
{
    private readonly PatientService _patientService;
    private ObservableCollection<Patient> _patients;
    private Patient? _selectedPatient;
    private ObservableCollection<Prescription>? _prescriptions;
    private Prescription? _selectedPrescription;

    public MedicationManagementViewModel()
    {
        _patientService = new PatientService();
        _patients = new ObservableCollection<Patient>(_patientService.GetAllPatients());
        _selectedPatient = null;
        _prescriptions = null;
        _selectedPrescription = null;

        GiveMedicationCommand = new ViewModelCommand(ExecuteGiveMedicationCommand, CanExecuteGiveMedicationCommand);
    }

    public ObservableCollection<Patient> Patients
    {
        get => _patients;
        set
        {
            _patients = value;
            OnPropertyChanged(nameof(Patients));
        }
    }

    public Patient? SelectedPatient
    {
        get => _selectedPatient;
        set
        {
            _selectedPatient = value;
            OnPropertyChanged(nameof(SelectedPatient));
            if (value != null)
            {
                Prescriptions = new ObservableCollection<Prescription>(SelectedPatient.MedicalRecord.Prescriptions);
            }
        }
    }

    public ObservableCollection<Prescription>? Prescriptions
    {
        get => _prescriptions;
        set
        {
            _prescriptions = value;
            OnPropertyChanged(nameof(Prescriptions));
        }
    }

    public Prescription? SelectedPrescription
    {
        get => _selectedPrescription;
        set
        {
            _selectedPrescription = value;
            OnPropertyChanged(nameof(SelectedPrescription));
        }
    }

    public ICommand GiveMedicationCommand { get; }

    private void ExecuteGiveMedicationCommand(object obj)
    {
        throw new NotImplementedException();
    }

    private bool CanExecuteGiveMedicationCommand(object obj)
    {
        return SelectedPatient != null && SelectedPrescription != null;
    }
}