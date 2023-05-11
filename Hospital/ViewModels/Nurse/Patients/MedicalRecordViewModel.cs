using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using Hospital.Models.Patient;

namespace Hospital.ViewModels.Nurse.Patients;

internal class MedicalRecordViewModel : ViewModelBase
{
    public ICommand CloseDialogCommand { get; }

    public MedicalRecordViewModel()
    {
        // dummy constructor
    }
    public MedicalRecordViewModel(MedicalRecord selectedPatientMedicalRecord)
    {
        Weight = selectedPatientMedicalRecord.Weight.ToString();
        Height = selectedPatientMedicalRecord.Height.ToString();
        MedicalHistory = selectedPatientMedicalRecord.MedicalHistory.Conditions;
        Allergies = selectedPatientMedicalRecord.Allergies.Conditions;

        CloseDialogCommand = new ViewModelCommand(ExecuteCloseDialogCommand);
    }

    public string Weight { get; }
    public string Height { get; }
    public List<string> MedicalHistory { get; }
    public List<string> Allergies { get; }

    private void ExecuteCloseDialogCommand(object obj)
    {
        Application.Current.Windows[1]?.Close();
    }
}