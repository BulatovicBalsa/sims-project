using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Hospital.Models.Patient;
using Hospital.Repositories.Patient;
using Hospital.Services;

namespace Hospital.ViewModels.Nurse.Medication;
public class MedicationManagementViewModel : ViewModelBase
{
    private readonly PatientService _patientService;
    private readonly MedicationService _medicationService;
    private readonly PatientRepository _patientRepository;
    private ObservableCollection<Patient> _patients;
    private Patient? _selectedPatient;
    private ObservableCollection<Prescription>? _patientPrescriptions;
    private Prescription? _selectedPrescription;

    public MedicationManagementViewModel()
    {
        _patientService = new PatientService();
        _medicationService = new MedicationService();
        _patientRepository = PatientRepository.Instance;
        _patients = new ObservableCollection<Patient>(_patientService.GetAllPatients());
        _selectedPatient = null;
        _patientPrescriptions = null;
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
                PatientPrescriptions = new ObservableCollection<Prescription>(SelectedPatient.MedicalRecord.Prescriptions);
            }
        }
    }

    public ObservableCollection<Prescription>? PatientPrescriptions
    {
        get => _patientPrescriptions;
        set
        {
            _patientPrescriptions = value;
            OnPropertyChanged(nameof(PatientPrescriptions));
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
        if (_medicationService.GetMedicationStock(SelectedPrescription.Medication) == 0)
        {
            MessageBox.Show("Selected medication is out of stock!", "Error");
            return;
        }
        else if (!SelectedPrescription.CanBeDispensed())
        {
            MessageBox.Show("It is still too early to dispense selected medicine!", "Error");
            return;
        }

        SelectedPrescription.LastUsed = DateTime.Now;
        _patientRepository.Update(SelectedPatient);
        _medicationService.DecrementMedicationStock(SelectedPrescription.Medication);
        MessageBox.Show("Medication successfully dispensed!", "Success");

        ResetInput();
    }

    private bool CanExecuteGiveMedicationCommand(object obj)
    {
        return SelectedPatient != null && SelectedPrescription != null;
    }

    private void ResetInput()
    {
        SelectedPatient = null;
        PatientPrescriptions = null;
        SelectedPrescription = null;
    }
}