using System.Windows;
using Hospital.Core.Workers.Models;
using Hospital.GUI.ViewModels.PatientHealthcare;

namespace Hospital.GUI.Views.PatientHealthcare;

public partial class VisitHospitalizedPatientsDialog : Window
{
    public VisitHospitalizedPatientsDialog(Doctor doctor)
    {
        DataContext = new VisitHospitalizedPatientsViewModel(doctor);
        InitializeComponent();
    }
}