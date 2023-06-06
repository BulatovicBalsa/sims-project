using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Hospital.DTOs;
using Hospital.Models.Doctor;
using Hospital.Services;
using Hospital.Views;

namespace Hospital.ViewModels;

public class VisitHospitalTreatmentPatientsViewModel : ViewModelBase
{
    private readonly Doctor _doctor;
    private readonly HospitalTreatmentService _hospitalTreatmentService = new();
    private Visibility _dataGridVisibility;
    private ObservableCollection<MedicalVisitDto> _medicalVisits;
    private Visibility _progressVisibility;

    public VisitHospitalTreatmentPatientsViewModel(Doctor doctor)
    {
        _doctor = doctor;
        _medicalVisits =
            new ObservableCollection<MedicalVisitDto>(_hospitalTreatmentService.GetHospitalizedPatients(doctor));
        ModifyTherapyCommand = new RelayCommand<string>(ModifyTherapy);
    }

    public ObservableCollection<MedicalVisitDto> MedicalVisits
    {
        get => _medicalVisits;
        set
        {
            _medicalVisits = value;
            OnPropertyChanged(nameof(MedicalVisits));
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

    private void ModifyTherapy(string patientId)
    {
        MedicalVisitDto selectedVisit = null;
        foreach (var medicalVisit in MedicalVisits)
            if (medicalVisit.Patient.Id == patientId)
                selectedVisit = medicalVisit;

        if (selectedVisit is null) throw new InvalidOperationException();

        ProgressVisibility = Visibility.Visible;
        
        var dialog = new ModifyTherapyDialog(selectedVisit!.Patient, selectedVisit.Referral);
        dialog.ShowDialog();

        MedicalVisits =
            new ObservableCollection<MedicalVisitDto>(_hospitalTreatmentService.GetHospitalizedPatients(_doctor));
        ProgressVisibility = Visibility.Hidden;
    }
}