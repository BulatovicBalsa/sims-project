using System.Windows;
using System.Windows.Input;

namespace Hospital.ViewModels.Books;

public partial class AdvancedBookDetailsDialog : Window
{
    public AdvancedBookDetailsDialog()
    {
        InitializeComponent();
    }

    private void BtnClose_OnClick(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void BtnMinimize_OnClick(object sender, RoutedEventArgs e)
    {
        WindowState = WindowState.Minimized;
    }

    private void ControlBar_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        DragMove();
    }
}