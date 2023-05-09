using GalaSoft.MvvmLight.Command;
using Hospital.Coordinators;
using Hospital.Models.Examination;
using Hospital.Models.Patient;
using Hospital.Views;
using System.Windows;
using System.Windows.Input;

namespace Hospital.ViewModels;

public class PerformExaminationDialogViewModel : ViewModelBase
{
    private readonly DoctorService _doctorService = new();

    private string _anamnesis;

    public PerformExaminationDialogViewModel(Examination examinationToPerform, Patient patientOnExamination)
    {
        _examinationToPerform = examinationToPerform;
        _patientOnExamination = patientOnExamination;
        Anamnesis = _examinationToPerform.Anamnesis;

        UpdateExaminationCommand = new RelayCommand(UpdateExamination);
        FinishExaminationCommand = new RelayCommand<Window>(FinishExamination);
    }

    private Examination _examinationToPerform { get; }
    private Patient _patientOnExamination { get; }

    public string Anamnesis
    {
        get => _anamnesis;
        set
        {
            _anamnesis = value;
            OnPropertyChanged(nameof(Anamnesis));
        }
    }

    public string FirstName => _patientOnExamination.FirstName;

    public string LastName => _patientOnExamination.LastName;

    public string Jmbg => _patientOnExamination.Jmbg;

    public ICommand UpdateExaminationCommand { get; set; }
    public ICommand FinishExaminationCommand { get; set; }

    private void UpdateExamination()
    {
        _examinationToPerform.Anamnesis = Anamnesis;
        _doctorService.UpdateExamination(_examinationToPerform);
        MessageBox.Show("Succeed");
    }

    private void FinishExamination(Window window)
    {
        window.Close();
        var dialog = new ChangeDynamicRoomEquipment(_examinationToPerform.Room);
        dialog.ShowDialog();
    }
}