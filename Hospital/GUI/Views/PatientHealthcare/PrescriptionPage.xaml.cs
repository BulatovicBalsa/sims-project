using System.Windows.Controls;
using Hospital.Core.PatientHealthcare.Models;
using Hospital.GUI.ViewModels.Pharmacy;

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