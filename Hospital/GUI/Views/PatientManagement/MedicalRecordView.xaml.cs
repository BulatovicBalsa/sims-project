using System.Windows;
using System.Windows.Input;

namespace Hospital.GUI.Views.PatientManagement;

/// <summary>
///     Interaction logic for MedicalRecordView.xaml
/// </summary>
public partial class MedicalRecordView : Window
{
    public MedicalRecordView()
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