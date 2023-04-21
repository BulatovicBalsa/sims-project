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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Hospital.Views
{
    /// <summary>
    /// Interaction logic for MedicalRecordDialog.xaml
    /// </summary>
    public partial class MedicalRecordDialog : Window
    {
        private Patient _patient;
        public MedicalRecordDialog(Patient patient, bool isEditable)
        {
            InitializeComponent();
            _patient = patient;
            var page = new MedicalRecordPage(_patient, isEditable);
            MedicalRecordFrame.Navigate(page);
            Title = page.Title + $"'s Medical Record";
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            SizeToContent = SizeToContent.WidthAndHeight;
        }
    }
}
