using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Hospital.DTOs;
using Hospital.Models.Doctor;
using Hospital.Models.Examination;
using Hospital.Models.Patient;
using Hospital.Services;
using Hospital.Views;

namespace Hospital.ViewModels;

public class VisitHospitalizedPatientsViewModel : ViewModelBase
{
    private readonly Doctor _doctor;
    private readonly HospitalTreatmentService _hospitalTreatmentService = new();
    private readonly PatientService _patientService = new();
    private Visibility _dataGridVisibility;
    private ObservableCollection<MedicalVisitDto> _medicalVisits;
    private Visibility _progressVisibility;

    public VisitHospitalizedPatientsViewModel(Doctor doctor)
    {
        _doctor = doctor;
        _medicalVisits =
            new ObservableCollection<MedicalVisitDto>(_hospitalTreatmentService.GetHospitalizedPatients(doctor));
        ModifyTherapyCommand = new RelayCommand<string>(ModifyTherapy);
        ReleasePatientCommand = new RelayCommand<string>(ReleasePatient);
    }

    public ObservableCollection<MedicalVisitDto> MedicalVisits
    {
        get => _medicalVisits;
        set
        {
            _medicalVisits = value;
            OnPropertyChanged(nameof(MedicalVisits));
            ProgressVisibility = Visibility.Hidden;
        }
    }

    public Visibility ProgressVisibility
    {
        get => _progressVisibility;
        set
        {
            _progressVisibility = value;
            OnPropertyChanged(nameof(ProgressVisibility));
            DataGridVisibility = value switch
            {
                Visibility.Hidden => Visibility.Visible,
                Visibility.Visible => Visibility.Hidden,
                _ => DataGridVisibility
            };
        }
    }

    public Visibility DataGridVisibility
    {
        get => _dataGridVisibility;
        set
        {
            _dataGridVisibility = value;
            OnPropertyChanged(nameof(DataGridVisibility));
        }
    }

    public ICommand ModifyTherapyCommand { get; set; }
    public ICommand ReleasePatientCommand { get; set; }

    private void ReleasePatient(string patientId)
    {
        var selectedVisit = GetMedicalVisitDto(patientId);
        var patient = _patientService.GetPatientById(patientId);

        patient!.ReleaseHospitalTreatmentReferral(selectedVisit.Referral);
        _patientService.UpdatePatient(patient!);

        MedicalVisits =
            new ObservableCollection<MedicalVisitDto>(_hospitalTreatmentService.GetHospitalizedPatients(_doctor));

        var dialogResult = MessageBox.Show("Patient released, do you want to create examination in 10 days?", "Confirmation",
            MessageBoxButton.YesNo, MessageBoxImage.Question);

        CreateExaminationInTenDays(dialogResult, patient);
    }

    private void CreateExaminationInTenDays(MessageBoxResult dialogResult, Patient patient)
    {
        if (dialogResult == MessageBoxResult.No) return;

        var createExaminationDialog =
            new ModifyExaminationDialog(_doctor, new ObservableCollection<Examination>(), null, DateTime.Today.AddDays(10), patient);
        var result = createExaminationDialog.ShowDialog();

        if (!result.GetValueOrDefault()) return;
        MessageBox.Show("Successfully added examination", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
    }

    private void ModifyTherapy(string patientId)
    {
        var selectedVisit = GetMedicalVisitDto(patientId);

        var dialog = new ModifyTherapyDialog(selectedVisit.Patient, selectedVisit.Referral);
        dialog.ShowDialog();

        MedicalVisits =
            new ObservableCollection<MedicalVisitDto>(_hospitalTreatmentService.GetHospitalizedPatients(_doctor));
    }

    private MedicalVisitDto GetMedicalVisitDto(string patientId)
    {
        MedicalVisitDto selectedVisit = null;
        foreach (var medicalVisit in MedicalVisits)
            if (medicalVisit.Patient.Id == patientId)
                selectedVisit = medicalVisit;

        if (selectedVisit is null) throw new InvalidOperationException();

        ProgressVisibility = Visibility.Visible;
        return selectedVisit;
    }
}