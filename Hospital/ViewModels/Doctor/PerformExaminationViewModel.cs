using GalaSoft.MvvmLight.Command;
using Hospital.Coordinators;
using Hospital.Models.Examination;
using Hospital.Models.Patient;
using Hospital.Views;
using System.Windows;
using System.Windows.Input;
using Hospital.Services;

namespace Hospital.ViewModels;

public class PerformExaminationViewModel : ViewModelBase
{
    private readonly DoctorService _doctorService = new();
    private readonly ExaminationService _examinationService = new();

    private string _anamnesis;
    public bool IsReferralAdded { get; set; }

    public PerformExaminationViewModel(Examination examinationToPerform, Patient patientOnExamination)
    {
        _examinationToPerform = examinationToPerform;
        PatientOnExamination = patientOnExamination;
        Anamnesis = _examinationToPerform.Anamnesis;
        IsReferralAdded = false;

        UpdateAnamnesisCommand = new RelayCommand(UpdateAnamnesis);
        FinishExaminationCommand = new RelayCommand<Window>(FinishExamination);
        CreateReferralCommand = new RelayCommand(CreateReferral);
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

    private void UpdateAnamnesis()
    {
        _examinationToPerform.Anamnesis = Anamnesis;
        _examinationService.UpdateExamination(_examinationToPerform, false);
        var result = MessageBox.Show("Anamnesis Saved, do you want to create prescriptions?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
        if (result == MessageBoxResult.Yes)
        {
            var dialog = new CreatePrescriptionDialog(PatientOnExamination);
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
        if (IsReferralAdded) //Consider Allowing Referral Updates
        {
            MessageBox.Show("Referral is already added");
            return;
        }
        Referral createdReferral = new();
        var dialog = new CreateReferralDialog(createdReferral);
        dialog.ShowDialog();
        if (!createdReferral.IsDefault()) IsReferralAdded = true;
        PatientOnExamination.Referrals.Add(createdReferral);

        new PatientService().UpdatePatient(PatientOnExamination);
    }
}