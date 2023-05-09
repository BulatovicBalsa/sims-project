using GalaSoft.MvvmLight.Command;
using Hospital.Coordinators;
using Hospital.Models.Examination;
using Hospital.Models.Patient;
using Hospital.Views;
using System.Windows;
using System.Windows.Input;

namespace Hospital.ViewModels;

public class PerformExaminationViewModel : ViewModelBase
{
    private readonly DoctorService _doctorService = new();

    private string _anamnesis;

    public PerformExaminationViewModel(Examination examinationToPerform, Patient patientOnExamination)
    {
        _examinationToPerform = examinationToPerform;
        PatientOnExamination = patientOnExamination;
        Anamnesis = _examinationToPerform.Anamnesis;

        UpdateAnamnesisCommand = new RelayCommand(UpdateAnamnesis);
        FinishExaminationCommand = new RelayCommand<Window>(FinishExamination);
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

    private void UpdateAnamnesis()
    {
        _examinationToPerform.Anamnesis = Anamnesis;
        _doctorService.UpdateExamination(_examinationToPerform);
        MessageBox.Show("Succeed");
    }

    private void FinishExamination(Window window)
    {
        window.Close();
        var dialog = new ChangeDynamicRoomEquipment(_examinationToPerform.Room!);
        dialog.ShowDialog();
    }
}