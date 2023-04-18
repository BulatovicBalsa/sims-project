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
    }
}
