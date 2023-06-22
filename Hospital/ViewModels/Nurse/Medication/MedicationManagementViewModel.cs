using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Hospital.DTOs;
using Hospital.Repositories.Patient;
using Hospital.Services;
using Hospital.Views.Librarian.Medication;

namespace Hospital.ViewModels.Librarian.Medication;

using Hospital.Models.Patient;
public class MedicationManagementViewModel : ViewModelBase
{
    private readonly PatientService _patientService;
    private readonly MedicationService _medicationService;
    private readonly PatientRepository _patientRepository;
    private readonly MedicationOrderService _medicationOrderService;
    private ObservableCollection<Patient> _patients;
    private Patient? _selectedPatient;
    private ObservableCollection<Prescription>? _patientPrescriptions;
    private Prescription? _selectedPrescription;
    private ObservableCollection<MedicationOrderQuantityDto> _medicationOrderQuantities;

    public MedicationManagementViewModel()
    {
        _patientService = new PatientService();
        _medicationService = new MedicationService();
        _patientRepository = PatientRepository.Instance;
        _medicationOrderService = new MedicationOrderService();
        _patients = new ObservableCollection<Patient>(_patientService.GetAllPatients());
        _selectedPatient = null;
        _patientPrescriptions = null;
        _selectedPrescription = null;
        _medicationOrderQuantities = new ObservableCollection<MedicationOrderQuantityDto>(_medicationService
            .GetLowStockMedication().Select(medication =>
                new MedicationOrderQuantityDto(medication.Id, medication.Name, medication.Stock, 0)));

        GiveMedicationCommand = new ViewModelCommand(ExecuteGiveMedicationCommand, CanExecuteGiveMedicationCommand);
        OrderMedicationCommand = new ViewModelCommand(ExecuteOrderMedicationCommand, CanExecuteOrderMedicationCommand);
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

    public ObservableCollection<MedicationOrderQuantityDto> MedicationOrderQuantities
    {
        get => _medicationOrderQuantities;
        set
        {
            _medicationOrderQuantities = value;
            OnPropertyChanged(nameof(MedicationOrderQuantities));
        }
    }

    public ICommand GiveMedicationCommand { get; }
    public ICommand OrderMedicationCommand { get; }

    private void ExecuteGiveMedicationCommand(object obj)
    {
        if (_medicationService.GetMedicationStock(SelectedPrescription.Medication) == 0)
        {
            MessageBox.Show("Selected medication is out of stock!", "Error");
            return;
        }
        if (!SelectedPrescription.CanBeDispensed())
        {
            MessageBox.Show("It is still too early to dispense selected medicine!", "Error");
            return;
        }

        SelectedPrescription.LastUsed = DateTime.Now;
        _patientRepository.Update(SelectedPatient);
        _medicationService.DecrementMedicationStock(SelectedPrescription.Medication);

        if (MessageBox.Show("Medication successfully dispensed!\nIs the patient feeling better?", "Success", MessageBoxButton.YesNo) == MessageBoxResult.No)
        {
            OpenScheduleExaminationDialog();
        }

        ResetInput();
    }

    private void OpenScheduleExaminationDialog()
    {
        var prescriptionExaminationViewModel =
            new PrescriptionExaminationViewModel(SelectedPrescription.DoctorId, SelectedPatient.Id);
        var scheduleExaminationDialog = new PrescriptionExaminationDialogView()
        {
            DataContext = prescriptionExaminationViewModel
        };

        scheduleExaminationDialog.ShowDialog();
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

    private void ExecuteOrderMedicationCommand(object obj)
    {
        var medicationToOrder = MedicationOrderQuantities.Where(elem => elem.OrderQuantity > 0).ToList();
        medicationToOrder.ForEach(order => _medicationOrderService.AddNewOrder(order));

        MessageBox.Show("Medication successfully ordered!", "Success");
        ResetOrderQuantities();
    }

    private void ResetOrderQuantities()
    {
        foreach (var medicationOrderQuantityDto in MedicationOrderQuantities)
        {
            medicationOrderQuantityDto.OrderQuantity = 0;
        }
    }
    private bool CanExecuteOrderMedicationCommand(object obj)
    {
        return !_medicationOrderQuantities.Any(elem => elem.OrderQuantity < 0);
    }
}