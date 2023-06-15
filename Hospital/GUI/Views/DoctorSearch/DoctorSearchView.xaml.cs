using System.Windows;
using Hospital.Core.PatientHealthcare.Models;
using Hospital.Core.Workers.Models;
using Hospital.GUI.ViewModels.DoctorSearch;
using Hospital.GUI.ViewModels.PatientHealthcare;
using Hospital.GUI.Views.Scheduling;

namespace Hospital.GUI.Views.DoctorSearch;

public partial class DoctorSearchView : Window
{
    private readonly Patient _patient;
    private readonly PatientViewModel _patientViewModel;
    private readonly DoctorSearchViewModel _viewModel;

    public DoctorSearchView(Patient patient, PatientViewModel patientViewModel)
    {
        InitializeComponent();

        _patient = patient;
        _viewModel = new DoctorSearchViewModel();
        DataContext = _viewModel;
        _patientViewModel = patientViewModel;
    }

    private void BtnMinimize_Click(object sender, RoutedEventArgs e)
    {
        WindowState = WindowState.Minimized;
    }

    private void BtnClose_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void BtnCreateExamination_Click(object sender, RoutedEventArgs e)
    {
        if (DoctorDataGrid.SelectedItem != null)
        {
            var selectedDoctor = (Doctor)DoctorDataGrid.SelectedItem;
            var examinationDialogView =
                new ExaminationDialogView(_patient, _patientViewModel, false, doctor: selectedDoctor);
            examinationDialogView.ShowDialog();
        }
        else
        {
            MessageBox.Show("Please select a doctor before creating an examination.", "Error", MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
    }
}