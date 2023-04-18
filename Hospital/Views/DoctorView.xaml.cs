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
    /// <summary>
    /// Interaction logic for DoctorView.xaml
    /// </summary>
    public partial class DoctorView : Window
    {
        ObservableCollection<Examination> Examinations = new ObservableCollection<Examination>();
        ObservableCollection<Patient> Patients = new ObservableCollection<Patient>();

        private readonly DoctorCoordinator _coordinator;
        private PatientRepository patientRepository = new PatientRepository();
        private ExaminationRepository examinationRepository = new ExaminationRepository(new Models.Patient.ExaminationChangesTracker());
        private Doctor Doctor;

        private bool isUserInput = true;
        private string placeholder = "Search...";

        public DoctorView(Doctor doctor)
        {
            InitializeComponent();
            this.Doctor = doctor;
            this.DataContext = this;
            
            _coordinator = new DoctorCoordinator(examinationRepository, patientRepository);
            
            Examinations = new ObservableCollection<Examination>(examinationRepository.GetExaminationsForNextThreeDays(doctor));
            Patients = new ObservableCollection<Patient>(_coordinator.GetViewedPatients(doctor));
            
            ExaminationsDataGrid.ItemsSource = Examinations;
            PatientsDataGrid.ItemsSource = Patients;
            
            isUserInput = false;
            SearchBox.Text = placeholder;

            this.SizeToContent = SizeToContent.Height;
        }

        private void BtnAddExamination_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new ExaminationDialog(Doctor, Examinations);

            dialog.WindowStyle = WindowStyle.ToolWindow;

            bool? result = dialog.ShowDialog();

            // Handle the result of the dialog
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

            // Handle the result of the dialog
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
                examinationRepository.Delete(examination, false);
            }catch(BusyDoctorException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            catch (BusyPatientException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            this.Examinations.Remove(examination);
            MessageBox.Show("Succeed");
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnViewMedicalRecord_Click(object sender, RoutedEventArgs e)
        {
            Patient? patient = PatientsDataGrid.SelectedItem as Patient;
            if (patient == null)
            {
                MessageBox.Show("Please select examination in order to delete it");
                return;
            }

            var dialog = new MedicalRecordDialog(patient);
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

            // Filter the patient list based on the search text
            List<Patient> filteredPatients = Patients.Where(patient =>
                patient.FirstName.ToLower().Contains(searchText) ||
                patient.LastName.ToLower().Contains(searchText) ||
                patient.Jmbg.ToLower().ToLower().Contains(searchText) ||
                patient.Id.ToLower().Contains(searchText)).ToList();
            // Update the data context of the patient grid to show the filtered patients
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
    }
}
