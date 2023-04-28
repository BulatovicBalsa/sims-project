using Hospital.Models.Patient;
using Hospital.ViewModels;
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
    /// Interaction logic for ExaminationRecommenderDialogPatientView.xaml
    /// </summary>
    public partial class ExaminationRecommenderDialogPatientView : Window
    {
        private readonly ExaminationRecommenderDialogPatientViewModel _viewModel;
        private readonly PatientViewModel _patientViewModel;
        private readonly Patient _patient;
        public ExaminationRecommenderDialogPatientView(Patient patient, PatientViewModel patientViewModel)
        {
            InitializeComponent();
            _viewModel = new ExaminationRecommenderDialogPatientViewModel(patient);
            _viewModel.RequestClose += CloseWindow;
            DataContext = _viewModel;
            _patientViewModel = patientViewModel;
            _patient = patient;
        }

        private void CloseWindow()
        {
            Close();
        }
        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void BtnSelect_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.SelectCommand.Execute(null);
            _patientViewModel.RefreshExaminations(_patient);
            Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.CancelCommand.Execute(null);
            Close();
        }

        private void BtnFind_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.FindCommand.Execute(null);
        }
    }
}
