using Hospital.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Hospital.Views
{
    public partial class PatientMedicalRecordView : Window
    {
        public PatientMedicalRecordView() 
        {
            InitializeComponent();

            DataContext = new PatientMedicalRecordViewModel();
        }
    }
}
