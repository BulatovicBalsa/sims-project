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
    /// <summary>
    /// Interaction logic for PerformExaminationDialog.xaml
    /// </summary>
    public partial class PerformExaminationDialog : Window
    {
        public PerformExaminationDialog(Patient patient)
        {
            InitializeComponent();
            loadMedicalRecordFrame(patient);
            this.SizeToContent = SizeToContent.WidthAndHeight;
            this.DataContext = patient;
            this.WindowStartupLocation = WindowStartupLocation.CenterOwner;
        }

        private void loadMedicalRecordFrame(Patient patient)
        {
            var dialog = new MedicalRecordDialog(patient, true);
            dialog.WindowStyle = WindowStyle.None;
            dialog.Show();
            dialog.Close();
            var temp = dialog.Content;
            MedicalRecordFrame.Content = new ContentControl() { Content = temp };
        }
    }
}
