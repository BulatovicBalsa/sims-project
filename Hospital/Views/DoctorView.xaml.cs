using Hospital.Models.Doctor;
using Hospital.Models.Examination;
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
        ObservableCollection<Examination> Examinations = new ObservableCollection<Examination>() { new Examination(new Doctor(), new Models.Patient.Patient(), false, DateTime.Now)};

        private PatientRepository patientRepository = new PatientRepository();
        private ExaminationRepository examinationRepository = new ExaminationRepository(new Models.Patient.ExaminationChangesTracker());
        private Doctor Doctor;

        public DoctorView(Doctor doctor)
        {
            InitializeComponent();
            this.Doctor = doctor;
            this.DataContext = this;
            ExaminationsDataGrid.ItemsSource = Examinations;

            foreach (var examination in examinationRepository.GetExaminationsForNextThreeDays(doctor))
            {
                Examinations.Add(examination);
            }
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
            else
            {
                // The dialog was closed without a result
            }
        }

        private void BtnUpdateExamination_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnDeleteExamination_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
