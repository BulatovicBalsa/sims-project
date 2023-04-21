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

        public PerformExaminationDialog(Examination examinationToPerform, Patient patientOnExamination)
        {
            InitializeComponent();

            _examinationToPerform = examinationToPerform;
            _patientOnExamination = patientOnExamination;

            loadMedicalRecordFrame();
            ConfigDialog();
        }

        private void ConfigDialog()
        {
            SizeToContent = SizeToContent.WidthAndHeight;
            DataContext = _patientOnExamination;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void loadMedicalRecordFrame()
        {
            var dialog = new MedicalRecordDialog(_patientOnExamination, true);
            dialog.WindowStyle = WindowStyle.None;
            dialog.Show();
            dialog.Close();
            var temp = dialog.Content;
            MedicalRecordFrame.Content = new ContentControl() { Content = temp };
        }
    }
}
