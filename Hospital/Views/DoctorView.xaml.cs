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
        ObservableCollection<Examination> Examinations = new ObservableCollection<Examination>();

        private PatientRepository patientRepository = new PatientRepository();
        private ExaminationRepository examinationRepository = new ExaminationRepository(new Models.Patient.ExaminationChangesTracker());
        private Doctor Doctor;

        public DoctorView(Doctor doctor)
        {
            InitializeComponent();
            this.Doctor = doctor;
            this.DataContext = this;

            foreach (var examination in examinationRepository.GetExaminationsForNextThreeDays(doctor))
            {
                Examinations.Add(examination);
            }
        }

        private void BtnAddExamination_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnUpdateExamination_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnDeleteExamination_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
