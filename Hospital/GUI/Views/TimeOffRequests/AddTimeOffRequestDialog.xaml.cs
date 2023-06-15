using System.Windows;
using Hospital.Core.Workers.Models;
using Hospital.GUI.ViewModels.TimeOffRequests;

namespace Hospital.GUI.Views.TimeOffRequests;

public partial class AddTimeOffRequestDialog : Window
{
    public AddTimeOffRequestDialog(Doctor doctor)
    {
        DataContext = new AddTimeOffRequestViewModel(doctor);
        InitializeComponent();
    }
}