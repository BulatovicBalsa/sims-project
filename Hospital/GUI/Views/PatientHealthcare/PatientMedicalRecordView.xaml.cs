using System.Windows;
using Hospital.Core.PatientHealthcare.Models;
using Hospital.GUI.ViewModels.PatientHealthcare;

namespace Hospital.GUI.Views.PatientHealthcare;

public partial class PatientMedicalRecordView : Window
{
    public PatientMedicalRecordView(Patient patient)
    {
        InitializeComponent();

        DataContext = new PatientMedicalRecordViewModel(patient);
    }

    private void BtnClose_Click(object sender, RoutedEventArgs e)
    {
        var dialogWindow = GetWindow(this);
        dialogWindow.Close();
    }

    private void BtnMinimize_Click(object sender, RoutedEventArgs e)
    {
        WindowState = WindowState.Minimized;
    }
}