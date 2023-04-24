using Hospital.Coordinators;
using Hospital.Models.Examination;
using Hospital.Models.Patient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Hospital.Views
{
    public partial class PerformExaminationDialog : Window
    {
        private Examination _examinationToPerform { get; set; }
        private Patient _patientOnExamination { get; set; }
        private readonly DoctorCoordinator _doctorCoordinator = new DoctorCoordinator();

        public PerformExaminationDialog(Examination examinationToPerform, Patient patientOnExamination)
        {
            InitializeComponent();

            _examinationToPerform = examinationToPerform;
            _patientOnExamination = patientOnExamination;

            loadMedicalRecordFrame();
            ConfigDialog();
            AnamnesisTextBox.DataContext = _examinationToPerform;
        }

        private void ConfigDialog()
        {
            SizeToContent = SizeToContent.WidthAndHeight;
            DataContext = _patientOnExamination;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            Title = $"{_patientOnExamination.FirstName} {_patientOnExamination.LastName}'s Examination";
        }

        private void loadMedicalRecordFrame()
        {
            var dialog = new MedicalRecordPage(_patientOnExamination, true);
            MedicalRecordFrame.Navigate(dialog);
        }
    }
}
