using Hospital.Coordinators;
using Hospital.Exceptions;
using Hospital.Models.Doctor;
using Hospital.Models.Examination;
using Hospital.Models.Patient;
using Hospital.Repositories.Doctor;
using Hospital.Repositories.Examinaton;
using Hospital.Repositories.Patient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Hospital.Views
{
    public partial class DoctorView : Window
    {
        private ObservableCollection<Examination> Examinations;
        private ObservableCollection<Patient> Patients;

        private Doctor Doctor;
        private bool isUserInput = true;

        private readonly DoctorCoordinator _coordinator = new DoctorCoordinator();
        private readonly string placeholder = "Search...";

        public DoctorView(Doctor doctor)
        {
            InitializeComponent();
            Doctor = doctor;
            DataContext = this;
            
            Examinations = new ObservableCollection<Examination>(_coordinator.GetExaminationsForNextThreeDays(doctor));
            Patients = new ObservableCollection<Patient>(_coordinator.GetViewedPatients(doctor));
            
            ExaminationsDataGrid.ItemsSource = Examinations;
            PatientsDataGrid.ItemsSource = Patients;
            
            isUserInput = false;
            SearchBox.Text = placeholder;

            SizeToContent = SizeToContent.Height;
        }

        private void BtnAddExamination_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new ExaminationDialog(Doctor, Examinations);
            dialog.WindowStyle = WindowStyle.ToolWindow;

            bool? result = dialog.ShowDialog();
            if (result == true)
            {
                MessageBox.Show("Succeed");
            }
        }

        private void BtnUpdateExamination_Click(object sender, RoutedEventArgs e)
        {
            Examination? examinationToChange = ExaminationsDataGrid.SelectedItem as Examination;
            if (examinationToChange == null)
            {
                MessageBox.Show("Please select examination in order to delete it");
                return;
            }

            var dialog = new ExaminationDialog(Doctor, Examinations, examinationToChange);

            dialog.WindowStyle = WindowStyle.ToolWindow;

            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                MessageBox.Show("Succeed");
            }
        }

        private void BtnDeleteExamination_Click(object sender, RoutedEventArgs e)
        {
            Examination? examination = ExaminationsDataGrid.SelectedItem as Examination;
            if (examination == null) {
                MessageBox.Show("Please select examination in order to delete it");
                return;
            }

            try
            {
                _coordinator.DeleteExamination(examination);
            }
            catch (DoctorNotBusyException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            catch (PatientNotBusyException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            Examinations.Remove(examination);
            MessageBox.Show("Succeed");
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void BtnViewMedicalRecord_Click(object sender, RoutedEventArgs e)
        {
            Patient? patient = PatientsDataGrid.SelectedItem as Patient;
            if (patient == null)
            {
                MessageBox.Show("Please select examination in order to delete it");
                return;
            }

            var dialog = new MedicalRecordDialog(patient, false);
            dialog.ShowDialog();
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!isUserInput)
            {
                isUserInput = true;
                return;
            }
            SearchBox.Foreground = Brushes.Black;
            string searchText = SearchBox.Text.ToLower();

            List<Patient> filteredPatients = Patients.Where(patient =>
                patient.FirstName.ToLower().Contains(searchText) ||
                patient.LastName.ToLower().Contains(searchText) ||
                patient.Jmbg.ToLower().ToLower().Contains(searchText) ||
                patient.Id.ToLower().Contains(searchText)).ToList();

            PatientsDataGrid.ItemsSource = filteredPatients;
        }

        private void SearchBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (SearchBox.Text == placeholder)
            {
                isUserInput = false;
                SearchBox.Text = "";
            }
        }

        private void SearchBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(SearchBox.Text))
            {
                isUserInput = false;
                SearchBox.Text = placeholder;
                SearchBox.Foreground = Brushes.Gray;
            }
        }

        private void BtnPerformExamination_Click(object sender, RoutedEventArgs e)
        {
            Examination? examinationToPerform = ExaminationsDataGrid.SelectedItem as Examination;
            if (examinationToPerform == null)
            {
                MessageBox.Show("Please select examination in order to perform it");
                return;
            }

            Patient patientOnExamination = _coordinator.GetPatient(examinationToPerform);

            /*if (!examination.IsPerfomable())
            {
                MessageBox.Show("Chosen examination can't be performed right now");
                return;
            }*/

            var dialog = new PerformExaminationDialog(examinationToPerform, patientOnExamination);
            dialog.ShowDialog();
        }
    }
}
