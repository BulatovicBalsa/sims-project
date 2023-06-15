using System.Windows;
using Hospital.GUI.ViewModels.PatientHealthcare;
using Hospital.PatientHealthcare.Models;

namespace Hospital.GUI.Views.PatientHealthcare;

public partial class CreateHospitalTreatmentReferralDialog : Window
{
    public CreateHospitalTreatmentReferralDialog(Patient patientOnExamination)
    {
        InitializeComponent();
        var referral = new HospitalTreatmentReferral();
        DataContext = new CreateHospitalTreatmentReferralViewModel(patientOnExamination, referral);
        PrescriptionsFrame.Navigate(new PrescriptionPage(patientOnExamination, referral));
    }
}