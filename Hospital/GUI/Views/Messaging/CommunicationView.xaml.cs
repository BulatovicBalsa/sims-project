using System.Windows;
using Hospital.Core.Accounts.DTOs;
using Hospital.GUI.ViewModels.Messaging;

namespace Hospital.GUI.Views.Messaging;

/// <summary>
///     Interaction logic for CommunicationView.xaml
/// </summary>
public partial class CommunicationView : Window
{
    public CommunicationView(PersonDTO loggedUser)
    {
        InitializeComponent();
        DataContext = new CommunicationViewModel(loggedUser);
    }

    private void BtnMinimize_Click(object sender, RoutedEventArgs e)
    {
        WindowState = WindowState.Minimized;
    }

    private void BtnClose_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
}