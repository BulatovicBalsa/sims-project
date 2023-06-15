using System.Windows;
using Hospital.Core.PatientHealthcare.Models;

namespace Hospital.GUI.Views.PatientHealthcare;

public partial class PrescriptionDialog : Window
{
    public PrescriptionDialog(Patient patientOnExamination)
    {
        InitializeComponent();
        PrescriptionsFrame.Navigate(new PrescriptionPage(patientOnExamination));
    }
}