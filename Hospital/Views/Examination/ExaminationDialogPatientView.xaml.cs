using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Hospital.Models.Doctor;
using Hospital.Models.Examination;
using Hospital.Models.Patient;
using Hospital.ViewModels;

namespace Hospital.Views
{
    public partial class ExaminationDialogView : Window
    {
        private readonly ExaminationViewModel _viewModel;

        public ExaminationDialogView(Patient patient,PatientViewModel patientViewModel, bool isUpdate, 
             Examination examination = null,Doctor doctor = null)
        {
            InitializeComponent();

            _viewModel = examination != null
                ? new ExaminationViewModel( patient,patientViewModel,examination)
                : new ExaminationViewModel( patient,patientViewModel,doctor: doctor);

            _viewModel.IsUpdate = isUpdate;
            this.DataContext = _viewModel;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Window dialogWindow = Window.GetWindow(this);
            dialogWindow.Close();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.SaveCommand.Execute(null);
        }
    }
}
