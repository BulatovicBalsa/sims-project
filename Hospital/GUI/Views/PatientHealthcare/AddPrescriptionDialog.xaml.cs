using System.Windows;
using Hospital.GUI.ViewModels.Pharmacy;
using Hospital.PatientHealthcare.Models;

namespace Hospital.GUI.Views.PatientHealthcare;

public partial class AddPrescriptionDialog : Window
{
    public AddPrescriptionDialog(Patient patientOnExamination,
        HospitalTreatmentReferral? referralToModify)
    {
        DataContext = new AddPrescriptionViewModel(patientOnExamination, referralToModify);
        InitializeComponent();
    }
}