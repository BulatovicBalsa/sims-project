using System.Windows;
using Hospital.Core.PatientHealthcare.Models;
using Hospital.GUI.ViewModels.PatientHealthcare;

namespace Hospital.GUI.Views.PatientHealthcare;

public partial class PerformExaminationDialog : Window
{
    public PerformExaminationDialog(Examination examinationToPerform,
        Patient patientOnExamination)
    {
        InitializeComponent();
        ConfigWindow(examinationToPerform, patientOnExamination);
        loadMedicalRecordFrame(patientOnExamination);
    }

    private void ConfigWindow(Examination examinationToPerform,
        Patient patientOnExamination)
    {
        SizeToContent = SizeToContent.WidthAndHeight;
        WindowStartupLocation = WindowStartupLocation.CenterScreen;
        Title = $"{patientOnExamination.FirstName} {patientOnExamination.LastName}'s Examination";
        DataContext = new PerformExaminationViewModel(examinationToPerform, patientOnExamination);
    }

    private void loadMedicalRecordFrame(Patient patientOnExamination)
    {
        var dialog = new MedicalRecordPage(patientOnExamination, true);
        MedicalRecordFrame.Navigate(dialog);
    }
}