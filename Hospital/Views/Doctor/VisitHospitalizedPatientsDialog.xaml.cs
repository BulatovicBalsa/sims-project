using System.Windows;
using Hospital.Models.Doctor;
using Hospital.ViewModels;

namespace Hospital.Views;

public partial class VisitHospitalizedPatientsDialog : Window
{
    public VisitHospitalizedPatientsDialog(Doctor doctor)
    {
        DataContext = new VisitHospitalizedPatientsViewModel(doctor);
        InitializeComponent();
    }
}