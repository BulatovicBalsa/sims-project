using System.Windows;
using Hospital.GUI.ViewModels.PatientFeedback;

namespace Hospital.GUI.Views.PatientFeedback;

/// <summary>
///     Interaction logic for HospitalFeedbackView.xaml
/// </summary>
public partial class HospitalFeedbackView : Window
{
    public HospitalFeedbackView()
    {
        InitializeComponent();
        DataContext = new HospitalFeedbackViewModel(this);
    }
}