using System.Windows;
using Hospital.Core.PatientHealthcare.Models;
using Hospital.GUI.ViewModels.PatientHealthcare;
using Hospital.GUI.ViewModels.Scheduling;

namespace Hospital.GUI.Views.Scheduling;

/// <summary>
///     Interaction logic for ExaminationRecommenderDialogPatientView.xaml
/// </summary>
public partial class ExaminationRecommenderDialogPatientView : Window
{
    private readonly Patient _patient;
    private readonly PatientViewModel _patientViewModel;
    private readonly ExaminationRecommenderDialogPatientViewModel _viewModel;

    public ExaminationRecommenderDialogPatientView(Patient patient, PatientViewModel patientViewModel)
    {
        InitializeComponent();
        _viewModel = new ExaminationRecommenderDialogPatientViewModel(patient);
        _viewModel.RequestClose += CloseWindow;
        DataContext = _viewModel;
        _patientViewModel = patientViewModel;
        _patient = patient;
    }

    private void CloseWindow()
    {
        Close();
    }

    private void BtnClose_Click(object sender, RoutedEventArgs e)
    {
        Application.Current.Shutdown();
    }

    private void BtnSelect_Click(object sender, RoutedEventArgs e)
    {
        _viewModel.SelectCommand.Execute(null);
        _patientViewModel.RefreshExaminations();
        Close();
    }

    private void BtnCancel_Click(object sender, RoutedEventArgs e)
    {
        _viewModel.CancelCommand.Execute(null);
        Close();
    }

    private void BtnFind_Click(object sender, RoutedEventArgs e)
    {
        _viewModel.FindCommand.Execute(null);
    }

    private void BtnMinimize_Click(object sender, RoutedEventArgs e)
    {
        GetWindow(this).WindowState = WindowState.Minimized;
    }
}