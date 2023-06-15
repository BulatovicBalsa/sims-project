using System.Windows;
using Hospital.Core.PatientHealthcare.Models;
using Hospital.GUI.ViewModels.Pharmacy;

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