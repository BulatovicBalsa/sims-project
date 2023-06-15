using System.Windows;
using Hospital.GUI.ViewModels.PatientHealthcare;
using Hospital.PatientHealthcare.Models;

namespace Hospital.GUI.Views.PatientHealthcare;

public partial class CreateReferralDialog : Window
{
    public CreateReferralDialog(Referral? referralToCreate)
    {
        DataContext = new CreateReferralViewModel(referralToCreate);
        InitializeComponent();
    }
}