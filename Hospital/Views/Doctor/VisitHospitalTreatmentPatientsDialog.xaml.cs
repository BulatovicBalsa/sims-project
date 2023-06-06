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
using Hospital.Models.Doctor;
using Hospital.ViewModels;

namespace Hospital.Views
{
    public partial class VisitHospitalTreatmentPatientsDialog : Window
    {
        public VisitHospitalTreatmentPatientsDialog(Doctor doctor)
        {
            DataContext = new VisitHospitalizedPatientsViewModel(doctor);
            InitializeComponent();
        }
    }
}
