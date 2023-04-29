using Hospital.Models.Patient;
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
        public PatientMedicalRecordView(Patient patient) 
        {
            InitializeComponent();

            DataContext = new PatientMedicalRecordViewModel(patient);
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Window dialogWindow = Window.GetWindow(this);
            dialogWindow.Close();
        }

        private void BtnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
    }
}
