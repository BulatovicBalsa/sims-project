using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Hospital.Exceptions;
using Hospital.Models.Examination;
using Hospital.Models.Patient;
using Hospital.Repositories.Examinaton;
using Hospital.Repositories.Patient;
using Hospital.ViewModels;

namespace Hospital.Views
{
    public partial class PatientView : Window
    {
        private PatientViewModel _viewModel;
        private Patient _patient;


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
            ExaminationDialogView examinationDialog = new ExaminationDialogView(_patient, _viewModel, false);
            examinationDialog.ShowDialog();
        }

        private void BtnUpdateExamination_Click(object sender, RoutedEventArgs e)
        {
            Examination examination = ExaminationsDataGrid.SelectedItem as Examination;

            if (examination != null)
            {
                ExaminationDialogView examinationDialog = new ExaminationDialogView(_patient, _viewModel, true, examination);
                examinationDialog.ShowDialog();
            }
        }

        private void BtnDeleteExamination_Click(object sender, RoutedEventArgs e)
        {
            Examination examination = ExaminationsDataGrid.SelectedItem as Examination;

            if (examination != null)
            {
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
                        MessageBox.Show("This user is now blocked due to too many changes made in the last 30 days.", "User Blocked");
                        Application.Current.Shutdown();
                    }
                }
            }
        }

        private void BtnFindExaminations_Click(object sender, RoutedEventArgs e)
        {
            ExaminationRecommenderDialogPatientView examinationRecommenderView = new ExaminationRecommenderDialogPatientView(_patient,_viewModel);
            examinationRecommenderView.ShowDialog();
        }

        private void BtnMedicalRecord_Click(object sender, RoutedEventArgs e)
        {
            PatientMedicalRecordView patientMedicalRecordView = new PatientMedicalRecordView(_patient);
            patientMedicalRecordView.ShowDialog();
        }

        private void BtnMinimize_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).WindowState = WindowState.Minimized;
        }

        private void BtnDoctorSearch_Click(object sender, RoutedEventArgs e)
        {
            var doctorSearchWindow = new DoctorSearchView(_patient,_viewModel);
            doctorSearchWindow.Show();
        }
    }
}
