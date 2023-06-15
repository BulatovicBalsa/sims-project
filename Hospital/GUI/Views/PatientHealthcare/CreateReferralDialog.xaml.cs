using System.Windows;
using Hospital.Core.PatientHealthcare.Models;
using Hospital.GUI.ViewModels.PatientHealthcare;

namespace Hospital.GUI.Views.PatientHealthcare;

public partial class CreateReferralDialog : Window
{
    public CreateReferralDialog(Referral? referralToCreate)
    {
        DataContext = new CreateReferralViewModel(referralToCreate);
        InitializeComponent();
    }
}