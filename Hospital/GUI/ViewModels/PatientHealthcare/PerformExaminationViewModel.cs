using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Hospital.Core.PatientHealthcare.Models;
using Hospital.Core.PatientHealthcare.Services;
using Hospital.GUI.Views.PatientHealthcare;
using Hospital.GUI.Views.PhysicalAssets;
using Hospital.PatientHealthcare.Services;

namespace Hospital.GUI.ViewModels.PatientHealthcare;

public class PerformExaminationViewModel : ViewModelBase
{
    private readonly ExaminationService _examinationService = new();

    private readonly Examination _examinationToPerform;

    private string _anamnesis;

    private ObservableCollection<HospitalTreatmentReferral> _hospitalTreatmentReferrals;
    private ObservableCollection<Referral> _referrals;

    public PerformExaminationViewModel(Examination examinationToPerform,
       Patient patientOnExamination)
    {
        _examinationToPerform = examinationToPerform;
        PatientOnExamination = patientOnExamination;
        Anamnesis = _examinationToPerform.Anamnesis;
        Referrals = new ObservableCollection<Referral>(PatientOnExamination.Referrals);
        HospitalTreatmentReferrals =
            new ObservableCollection<HospitalTreatmentReferral>(PatientOnExamination.HospitalTreatmentReferrals);

        UpdateAnamnesisCommand = new RelayCommand(UpdateAnamnesis);
        FinishExaminationCommand = new RelayCommand<Window>(FinishExamination);
        CreateReferralCommand = new RelayCommand(CreateReferral);
        CreateHospitalTreatmentReferralCommand = new RelayCommand(CreateHospitalTreatmentReferral);
    }

    public ObservableCollection<Referral> Referrals
    {
        get => _referrals;
        set
        {
            _referrals = value;
            OnPropertyChanged(nameof(Referrals));
        }
    }

    public ObservableCollection<HospitalTreatmentReferral> HospitalTreatmentReferrals
    {
        get => _hospitalTreatmentReferrals;
        set
        {
            _hospitalTreatmentReferrals = value;
            OnPropertyChanged(nameof(HospitalTreatmentReferrals));
        }
    }

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
        var result = MessageBox.Show("Anamnesis Saved, do you want to create prescriptions?", "Confirmation",
            MessageBoxButton.YesNo, MessageBoxImage.Question);
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
        Referrals = new ObservableCollection<Referral>(PatientOnExamination.Referrals);
    }

    private void CreateHospitalTreatmentReferral()
    {
        var dialog = new CreateHospitalTreatmentReferralDialog(PatientOnExamination);
        dialog.ShowDialog();
        HospitalTreatmentReferrals =
            new ObservableCollection<HospitalTreatmentReferral>(PatientOnExamination.HospitalTreatmentReferrals);
    }
}