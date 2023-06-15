using System.Windows.Controls;
using Hospital.GUI.ViewModels.Pharmacy;
using Hospital.PatientHealthcare.Models;

namespace Hospital.GUI.Views.PatientHealthcare;

public partial class PrescriptionPage : Page
{
    public PrescriptionPage(Patient patientOnExamination,
        HospitalTreatmentReferral? referralToModify = null)
    {
        DataContext = new PrescriptionViewModel(patientOnExamination, referralToModify);
        InitializeComponent();
    }
}