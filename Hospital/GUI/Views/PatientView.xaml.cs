using System;
using System.Windows;
using Hospital.GUI.ViewModels.PatientHealthcare;
using Hospital.GUI.Views.DoctorSearch;
using Hospital.GUI.Views.Notifications;
using Hospital.GUI.Views.PatientHealthcare;
using Hospital.GUI.Views.Scheduling;
using Hospital.PatientHealthcare.Models;
using Hospital.PatientHealthcare.Repositories;

namespace Hospital.GUI.Views;

public partial class PatientView : Window
{
    private readonly Patient _patient;
    private readonly PatientViewModel _viewModel;


    public PatientView(Patient patient)
    {
        InitializeComponent();

        _viewModel = new PatientViewModel(patient);
        _patient = patient;
        DataContext = _viewModel;
    }

    public PatientView()
    {
    }


    private void BtnClose_Click(object sender, RoutedEventArgs e)
    {
        Application.Current.Shutdown();
    }

    private void BtnAddExamination_Click(object sender, RoutedEventArgs e)
    {
        var examinationDialog = new ExaminationDialogView(_patient, _viewModel, false);
        examinationDialog.ShowDialog();
    }

    private void BtnUpdateExamination_Click(object sender, RoutedEventArgs e)
    {
        var examination = ExaminationsDataGrid.SelectedItem as Examination;

        if (examination != null)
        {
            var examinationDialog = new ExaminationDialogView(_patient, _viewModel, true, examination);
            examinationDialog.ShowDialog();
        }
    }

    private void BtnDeleteExamination_Click(object sender, RoutedEventArgs e)
    {
        var examination = ExaminationsDataGrid.SelectedItem as Examination;

        if (examination != null)
            try
            {
                _viewModel.DeleteExamination(examination);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
                if (ex.Message.Contains("Patient made too many changes in last 30 days"))
                {
                    _patient.IsBlocked = true;
                    PatientRepository.Instance.Update(_patient);
                    MessageBox.Show("This user is now blocked due to too many changes made in the last 30 days.",
                        "User Blocked");
                    Application.Current.Shutdown();
                }
            }
    }

    private void BtnFindExaminations_Click(object sender, RoutedEventArgs e)
    {
        var examinationRecommenderView = new ExaminationRecommenderDialogPatientView(_patient, _viewModel);
        examinationRecommenderView.ShowDialog();
    }

    private void BtnMedicalRecord_Click(object sender, RoutedEventArgs e)
    {
        var patientMedicalRecordView = new PatientMedicalRecordView(_patient);
        patientMedicalRecordView.ShowDialog();
    }

    private void BtnMinimize_Click(object sender, RoutedEventArgs e)
    {
        GetWindow(this).WindowState = WindowState.Minimized;
    }

    private void BtnDoctorSearch_Click(object sender, RoutedEventArgs e)
    {
        var doctorSearchWindow = new DoctorSearchView(_patient, _viewModel);
        doctorSearchWindow.Show();
    }

    private void BtnCreateNotification_Click(object sender, RoutedEventArgs e)
    {
        var patientNotificationView = new PatientNotificationView(_patient);
        patientNotificationView.ShowDialog();
    }

    private void BtnSaveNotificationTime_Click(object sender, RoutedEventArgs e)
    {
        if (int.TryParse(TxtNotificationTime.Text, out var notificationTime))
        {
            if (notificationTime >= 0)
            {
                _viewModel.SaveNotificationTime(notificationTime);
                MessageBox.Show("Notification time saved successfully!");
            }
            else
            {
                MessageBox.Show("Invalid notification time! Please enter a non-negative number.");
            }
        }
        else
        {
            MessageBox.Show("Invalid notification time! Please enter a valid number.");
        }
    }
}