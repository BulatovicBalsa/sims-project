using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Hospital.DTOs;
using Hospital.Models.Doctor;
using Hospital.Services;
using Hospital.Views;

namespace Hospital.ViewModels;

public class VisitHospitalTreatmentPatientsViewModel : ViewModelBase
{
    private readonly HospitalTreatmentService _hospitalTreatmentService = new();
    private ObservableCollection<MedicalVisitDto> _medicalVisits;
    private readonly Doctor _doctor;

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

    public ICommand ModifyTherapyCommand { get; set; }

    private void ModifyTherapy(string patientId)
    {
        MedicalVisitDto selectedVisit = null;
        foreach (var medicalVisit in MedicalVisits)
            if (medicalVisit.Patient.Id == patientId)
                selectedVisit = medicalVisit;

        if (selectedVisit is null) throw new InvalidOperationException();

        var dialog = new ModifyTherapyDialog(selectedVisit!.Patient, selectedVisit.Referral);
        dialog.ShowDialog();
        MedicalVisits = new ObservableCollection<MedicalVisitDto>(_hospitalTreatmentService.GetHospitalizedPatients(_doctor));
    }
}