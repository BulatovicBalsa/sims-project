using System.Windows;
using Hospital.Models.Doctor;
using Hospital.ViewModels.Feedback;

namespace Hospital.Views.Feedback;

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