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
        public MedicalRecordDialog(Patient patient)
        {
            InitializeComponent();
            this.DataContext = patient.MedicalRecord;
            this.Title = $"{patient.FirstName} {patient.LastName}";
        }
    }
}
