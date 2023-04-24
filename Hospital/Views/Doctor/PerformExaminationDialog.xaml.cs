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
        public PerformExaminationDialog(Examination examinationToPerform, Patient patientOnExamination)
        {
            InitializeComponent();

            loadMedicalRecordFrame(patientOnExamination);
            ConfigDialog(patientOnExamination);
        }

        private void ConfigDialog(Patient patientOnExamination)
        {
            SizeToContent = SizeToContent.WidthAndHeight;
            DataContext = patientOnExamination;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            Title = $"{patientOnExamination.FirstName} {patientOnExamination.LastName}'s Examination";
        }

        private void loadMedicalRecordFrame(Patient patientOnExamination)
        {
            var dialog = new MedicalRecordPage(patientOnExamination, true);
            MedicalRecordFrame.Navigate(dialog);
        }
    }
}
