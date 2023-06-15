using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Hospital.GUI.Views.PatientHealthcare;
using Hospital.PatientHealthcare.Models;
using Hospital.PatientHealthcare.Services;
using Hospital.Pharmacy.Models;

namespace Hospital.GUI.ViewModels.Pharmacy;

public class PrescriptionViewModel : ViewModelBase
{
    private readonly PatientService _patientService = new();
    private ObservableCollection<Prescription> _prescriptions = new();

    private Prescription? _selectedPrescription;

    public PrescriptionViewModel(Patient patientOnExamination,
        HospitalTreatmentReferral? referralToModify = null)
    {
        PatientOnExamination = patientOnExamination;
        ReferralToModify = referralToModify;
        PrescriptionsToModify = referralToModify is null
            ? PatientOnExamination.MedicalRecord.Prescriptions
            : referralToModify.Prescriptions;
        Prescriptions = new ObservableCollection<Prescription>(PrescriptionsToModify);

        AddPrescriptionCommand = new RelayCommand(AddPrescription);
        DeletePrescriptionCommand = new RelayCommand(DeletePrescription);
    }

    public Patient PatientOnExamination { get; set; }

    public Prescription? SelectedPrescription
    {
        get => _selectedPrescription;
        set
        {
            _selectedPrescription = value;
            OnPropertyChanged(nameof(SelectedPrescription));
        }
    }

    public ObservableCollection<Prescription> Prescriptions
    {
        get => _prescriptions;
        set
        {
            _prescriptions = value;
            PrescriptionsToModify.Clear();
            _prescriptions.ToList().ForEach(prescription => PrescriptionsToModify.Add(prescription));
            OnPropertyChanged(nameof(Prescriptions));
        }
    }

    public HospitalTreatmentReferral? ReferralToModify { get; set; }

    public List<Prescription> PrescriptionsToModify { get; set; }

    public ICommand AddPrescriptionCommand { get; set; }
    public ICommand DeletePrescriptionCommand { get; set; }

    private void DeletePrescription()
    {
        if (SelectedPrescription is null)
        {
            MessageBox.Show("You must select prescription in order to delete it", "", MessageBoxButton.OK,
                MessageBoxImage.Error);
            return;
        }

        Prescriptions.Remove(SelectedPrescription);
        Prescriptions = new ObservableCollection<Prescription>(Prescriptions);
        _patientService.UpdatePatient(PatientOnExamination);
    }

    private void AddPrescription()
    {
        var dialog = new AddPrescriptionDialog(PatientOnExamination, ReferralToModify);
        dialog.ShowDialog();
        Prescriptions = new ObservableCollection<Prescription>(PrescriptionsToModify);
        _patientService.UpdatePatient(PatientOnExamination);
    }
}