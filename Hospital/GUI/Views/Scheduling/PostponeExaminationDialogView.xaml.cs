using System.Windows;
using System.Windows.Input;

namespace Hospital.GUI.Views.Scheduling;

/// <summary>
///     Interaction logic for PostponeExaminationDialogView.xaml
/// </summary>
public partial class PostponeExaminationDialogView : Window
{
    public PostponeExaminationDialogView()
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