using System.Windows;
using Hospital.Core.Accounts.DTOs;
using Hospital.GUI.ViewModels.Messaging;

namespace Hospital.GUI.Views.Messaging;

/// <summary>
///     Interaction logic for CreateMessageView.xaml
/// </summary>
public partial class CreateMessageView : Window
{
    public CreateMessageViewModel ViewModel;

    public CreateMessageView(PersonDTO sender, PersonDTO recipient)
    {
        InitializeComponent();
        ViewModel = new CreateMessageViewModel(sender, recipient);
        DataContext = ViewModel;
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