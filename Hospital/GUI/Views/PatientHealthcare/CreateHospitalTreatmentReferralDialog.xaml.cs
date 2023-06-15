using System.Windows;
using Hospital.Core.PatientHealthcare.Models;
using Hospital.GUI.ViewModels.PatientHealthcare;

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