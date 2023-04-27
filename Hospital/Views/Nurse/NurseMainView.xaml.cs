using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;

namespace Hospital.Views.Nurse;

/// <summary>
///     Interaction logic for NurseMainView.xaml
/// </summary>
public partial class NurseMainView : Window
{
    public NurseMainView()
    {
        InitializeComponent();
    }

    [DllImport("user32.dll")]
    private static extern IntPtr SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);

    private void ControlBar_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        var helper = new WindowInteropHelper(this);
        SendMessage(helper.Handle, 161, 2, 0);
    }

    private void ControlBar_OnMouseEnter(object sender, MouseEventArgs e)
    {
        MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
    }

    private void BtnMinimize_OnClick(object sender, RoutedEventArgs e)
    {
        WindowState = WindowState.Minimized;
    }

    private void BtnClose_OnClick(object sender, RoutedEventArgs e)
    {
        Application.Current.Shutdown();
    }

    private void BtnMaximize_OnClick(object sender, RoutedEventArgs e)
    {
        WindowState = (WindowState == WindowState.Normal) ? WindowState.Maximized : WindowState.Normal;
    }
}
