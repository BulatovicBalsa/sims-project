using GalaSoft.MvvmLight.Command;
using Hospital.Models.Examination;
using Hospital.Models.Patient;
using Hospital.Views;
using System.Windows;
using System.Windows.Input;
using Hospital.Services;
using System;
using System.Collections.ObjectModel;

namespace Hospital.ViewModels;

public class PerformExaminationViewModel : ViewModelBase
{
    private readonly ExaminationService _examinationService = new();

    private string _anamnesis;
    private ObservableCollection<Referral> _referrals;
    public ObservableCollection<Referral> Referrals
    {
        get => _referrals;
        set
        {
            _referrals = value;
            OnPropertyChanged(nameof(Referrals));
        }
    }

    public PerformExaminationViewModel(Examination examinationToPerform, Patient patientOnExamination)
    {
        _examinationToPerform = examinationToPerform;
        PatientOnExamination = patientOnExamination;
        Anamnesis = _examinationToPerform.Anamnesis;
        Referrals = new ObservableCollection<Referral>(PatientOnExamination.Referrals);

        UpdateAnamnesisCommand = new RelayCommand(UpdateAnamnesis);
        FinishExaminationCommand = new RelayCommand<Window>(FinishExamination);
        CreateReferralCommand = new RelayCommand(CreateReferral);
        CreateHospitalTreatmentReferralCommand = new RelayCommand(CreateHospitalTreatmentReferral);
    }

    private readonly Examination _examinationToPerform;
    public Patient PatientOnExamination { get; }

    public string Anamnesis
    {
        get => _anamnesis;
        set
        {
            _anamnesis = value;
            OnPropertyChanged(nameof(Anamnesis));
        }
    }

    public ICommand UpdateAnamnesisCommand { get; set; }
    public ICommand FinishExaminationCommand { get; set; }
    public ICommand CreateReferralCommand { get; set; }
    public ICommand CreateHospitalTreatmentReferralCommand { get; set; }

    private void UpdateAnamnesis()
    {
        _examinationToPerform.Anamnesis = Anamnesis;
        _examinationService.UpdateExamination(_examinationToPerform, false);
        var result = MessageBox.Show("Anamnesis Saved, do you want to create prescriptions?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
        if (result == MessageBoxResult.Yes)
        {
            var dialog = new PrescriptionDialog(PatientOnExamination);
            dialog.ShowDialog();
        }
    }

    private void FinishExamination(Window window)
    {
        window.Close();
        var dialog = new ChangeDynamicRoomEquipment(_examinationToPerform.Room!);
        dialog.ShowDialog();
    }

    private void CreateReferral()
    {
        Referral createdReferral = new();
        var dialog = new CreateReferralDialog(createdReferral);
        dialog.ShowDialog();
        if (createdReferral.IsDefault()) return;

        PatientOnExamination.Referrals.Add(createdReferral);
        new PatientService().UpdatePatient(PatientOnExamination);
    }

    private void CreateHospitalTreatmentReferral()
    {
        var dialog = new CreateHospitalTreatmentReferralDialog(PatientOnExamination);
        dialog.ShowDialog();
    }
}