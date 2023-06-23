using Hospital.Models.Doctor;
using Hospital.Models.Patient;
using Hospital.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Hospital.Views
{
    public partial class DoctorSearchView : Window
    {
        private Patient _patient;
        private DoctorSearchViewModel _viewModel;
        private PatientViewModel _patientViewModel;

        public DoctorSearchView(Patient patient, PatientViewModel patientViewModel)
        {
            InitializeComponent();

            _patient = patient;
            _viewModel = new DoctorSearchViewModel();
            this.DataContext = _viewModel;
            _patientViewModel = patientViewModel;
        }

        private void BtnMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnCreateExamination_Click(object sender, RoutedEventArgs e)
        {
            if (DoctorDataGrid.SelectedItem != null)
            {
                var selectedDoctor = (Doctor)DoctorDataGrid.SelectedItem;
                var examinationDialogView = new ExaminationDialogView(_patient,_patientViewModel,false,doctor:selectedDoctor);
                examinationDialogView.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please select a member before creating an examination.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
