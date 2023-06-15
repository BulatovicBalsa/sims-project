using System.Windows;
using System.Windows.Input;

namespace Hospital.GUI.Views.Pharmacy;

/// <summary>
///     Interaction logic for PrescriptionExaminationDialogView.xaml
/// </summary>
public partial class PrescriptionExaminationDialogView : Window
{
    public PrescriptionExaminationDialogView()
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