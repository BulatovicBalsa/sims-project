using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Hospital.Models;
using Hospital.Models.Patient;
using Hospital.Repositories.Patient;
using Hospital.Services;

namespace Hospital.ViewModels;

public class AddPrescriptionViewModel : ViewModelBase
{
    private int _amount;
    private int _dailyUsage;

    private ObservableCollection<Medication> _medications;

    private ObservableCollection<MedicationTiming> _medicationTimings;

    private Medication? _selectedMedication;

    private MedicationTiming? _selectedMedicationTiming;
    private NotificationService _notificationService;

    public AddPrescriptionViewModel(Patient patientOnExamination)
    {
        PatientOnExamination = patientOnExamination;
        AddPrescriptionCommand = new RelayCommand<Window>(AddPrescription);
        Medications = new ObservableCollection<Medication>(MedicationRepository.Instance.GetAll());
        MedicationTimings = new ObservableCollection<MedicationTiming>(Enum.GetValues(typeof(MedicationTiming)).Cast<MedicationTiming>().ToList());
        _notificationService = new NotificationService();
        Amount = 1;
        DailyUsage = 1;
    }

    public Patient PatientOnExamination { get; set; }

    public int Amount
    {
        get => _amount;
        set
        {
            _amount = value;
            OnPropertyChanged(nameof(Amount));
        }
    }

    public int DailyUsage
    {
        get => _dailyUsage;
        set
        {
            _dailyUsage = value;
            OnPropertyChanged(nameof(DailyUsage));
        }
    }

    public ObservableCollection<Medication> Medications
    {
        get => _medications;
        set
        {
            _medications = value;
            OnPropertyChanged(nameof(Medications));
        }
    }

    public ObservableCollection<MedicationTiming> MedicationTimings
    {
        get => _medicationTimings;
        set
        {
            _medicationTimings = value;
            OnPropertyChanged(nameof(MedicationTimings));
        }
    }

    public MedicationTiming? SelectedMedicationTiming
    {
        get => _selectedMedicationTiming;
        set
        {
            _selectedMedicationTiming = value;
            OnPropertyChanged(nameof(SelectedMedicationTiming));
        }
    }

    public Medication? SelectedMedication
    {
        get => _selectedMedication;
        set
        {
            _selectedMedication = value;
            OnPropertyChanged(nameof(SelectedMedication));
        }
    }

    public ICommand AddPrescriptionCommand { get; set; }

    private void AddPrescription(Window window)
    {
        if (SelectedMedication is null || SelectedMedicationTiming is null)
        {
            MessageBox.Show("You must select Medication Timing and Medication");
            return;
        }

        if (PatientOnExamination.IsAllergicTo(SelectedMedication))
        {
            MessageBox.Show($"Patient is allergic to {SelectedMedication}!!!");
            return;
        }

        var prescriptionToAdd = new Prescription(SelectedMedication, Amount, DailyUsage,
            SelectedMedicationTiming.GetValueOrDefault());
        PatientOnExamination.MedicalRecord.Prescriptions.Add(prescriptionToAdd);

        GenerateNotificationsForPrescription(PatientOnExamination, prescriptionToAdd);

        MessageBox.Show("Succeed");
        window.DialogResult = true;
    }

    private void GenerateNotificationsForPrescription(Patient patient, Prescription prescription)
    {
        DateTime startDate = prescription.IssuedDate;
        DateTime endDate = prescription.IssuedDate.AddDays(prescription.Amount);

        var notifications = Enumerable.Range(0, (endDate - startDate).Days)
            .Select(offset => startDate.AddDays(offset))
            .Select(date => new Notification(patient, prescription, date))
            .ToList();

        notifications.ForEach(notification => _notificationService.Send(notification));
    }
}