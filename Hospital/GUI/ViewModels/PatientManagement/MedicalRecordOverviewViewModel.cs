using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using Hospital.Core.PatientHealthcare.Models;

namespace Hospital.GUI.ViewModels.PatientManagement;

internal class MedicalRecordOverviewViewModel : ViewModelBase
{
    public MedicalRecordOverviewViewModel()
    {
        // dummy constructor
    }

    public MedicalRecordOverviewViewModel(MedicalRecord selectedPatientMedicalRecord)
    {
        Weight = selectedPatientMedicalRecord.Weight.ToString();
        Height = selectedPatientMedicalRecord.Height.ToString();
        MedicalHistory = selectedPatientMedicalRecord.MedicalHistory.Conditions;
        Allergies = selectedPatientMedicalRecord.Allergies.Conditions;

        CloseDialogCommand = new ViewModelCommand(ExecuteCloseDialogCommand);
    }

    public ICommand CloseDialogCommand { get; }

    public string Weight { get; }
    public string Height { get; }
    public List<string> MedicalHistory { get; }
    public List<string> Allergies { get; }

    private void ExecuteCloseDialogCommand(object obj)
    {
        Application.Current.Windows[1]?.Close();
    }
}