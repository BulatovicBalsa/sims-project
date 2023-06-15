using System.Windows;
using Hospital.PatientHealthcare.Models;

namespace Hospital.GUI.Views.PatientHealthcare;

public partial class MedicalRecordDialog : Window
{
    private readonly Patient _patient;

    public MedicalRecordDialog(Patient patient, bool isEditable)
    {
        InitializeComponent();
        _patient = patient;

        ConfigWindow(isEditable);
    }

    private void ConfigWindow(bool isEditable)
    {
        var page = new MedicalRecordPage(_patient, isEditable);
        MedicalRecordFrame.Navigate(page);
        Title = page.Title + "'s Medical Record";
        WindowStartupLocation = WindowStartupLocation.CenterScreen;
        SizeToContent = SizeToContent.WidthAndHeight;
    }
}