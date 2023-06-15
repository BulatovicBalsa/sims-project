using System.Windows;
using Hospital.Core.Workers.Models;
using Hospital.GUI.ViewModels.PatientFeedback;

namespace Hospital.GUI.Views.PatientFeedback;

/// <summary>
///     Interaction logic for DoctorFeedbackView.xaml
/// </summary>
public partial class DoctorFeedbackView : Window
{
    public DoctorFeedbackView(Doctor doctor)
    {
        InitializeComponent();
        DataContext = new DoctorFeedbackViewModel(doctor, this);
    }
}