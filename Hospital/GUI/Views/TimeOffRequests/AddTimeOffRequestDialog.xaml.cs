using System.Windows;
using Hospital.GUI.ViewModels.TimeOffRequests;
using Hospital.Workers.Models;

namespace Hospital.GUI.Views.TimeOffRequests;

public partial class AddTimeOffRequestDialog : Window
{
    public AddTimeOffRequestDialog(Doctor doctor)
    {
        DataContext = new AddTimeOffRequestViewModel(doctor);
        InitializeComponent();
    }
}