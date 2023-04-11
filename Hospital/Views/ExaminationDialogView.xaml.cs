using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Hospital.Models.Doctor;
using Hospital.Models.Examination;
using Hospital.ViewModels;

namespace Hospital.Views
{
    public partial class ExaminationDialogView : Window
    {
        private readonly ExaminationDialogViewModel _viewModel;

        public ExaminationDialogView(PatientViewModel patientViewModel, bool isUpdate, 
             Examination examination = null, IEnumerable<Doctor> recommendedDoctors = null)
        {
            InitializeComponent();

            _viewModel = examination != null
                ? new ExaminationDialogViewModel(recommendedDoctors, examination, patientViewModel)
                : new ExaminationDialogViewModel(recommendedDoctors, patientViewModel);

            _viewModel.IsUpdate = isUpdate;
            this.DataContext = _viewModel;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.SaveCommand.Execute(null);
        }
    }
}
