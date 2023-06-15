﻿using System.Windows;
using System.Windows.Input;

namespace Hospital.GUI.Views.PatientHealthcare;

/// <summary>
///     Interaction logic for VisitingDialogView.xaml
/// </summary>
public partial class VisitingDialogView : Window
{
    public VisitingDialogView()
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