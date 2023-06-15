using System.Windows;
using Hospital.Core.PatientHealthcare.Models;
using Hospital.GUI.ViewModels.Notifications;

namespace Hospital.GUI.Views.Notifications;

/// <summary>
///     Interaction logic for PatientNotificationView.xaml
/// </summary>
public partial class PatientNotificationView : Window
{
    private readonly PatientNotificationViewModel _viewModel;

    public PatientNotificationView(Patient patient)
    {
        InitializeComponent();
        _viewModel = new PatientNotificationViewModel(patient);
        DataContext = _viewModel;
    }

    private void BtnMinimize_Click(object sender, RoutedEventArgs e)
    {
        WindowState = WindowState.Minimized;
    }

    private void BtnClose_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void CreateButton_Click(object sender, RoutedEventArgs e)
    {
        _viewModel.CreateNotification();
        Close();
    }
}