using System.Windows;
using Hospital.GUI.ViewModels.PatientHealthcare;
using Hospital.Workers.Models;

namespace Hospital.GUI.Views.PatientHealthcare;

public partial class VisitHospitalizedPatientsDialog : Window
{
    public VisitHospitalizedPatientsDialog(Doctor doctor)
    {
        DataContext = new VisitHospitalizedPatientsViewModel(doctor);
        InitializeComponent();
    }
}