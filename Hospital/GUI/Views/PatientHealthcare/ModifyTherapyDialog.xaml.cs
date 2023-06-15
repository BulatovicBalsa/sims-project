using System.Windows;
using Hospital.PatientHealthcare.Models;

namespace Hospital.GUI.Views.PatientHealthcare;

public partial class ModifyTherapyDialog : Window
{
    public ModifyTherapyDialog(Patient hospitalizedPatient, HospitalTreatmentReferral activeReferral)
    {
        InitializeComponent();
        PrescriptionsFrame.Navigate(new PrescriptionPage(hospitalizedPatient, activeReferral));
    }
}