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
        public ExaminationRecommenderDialogPatientView(Patient patient)
        {
            InitializeComponent();
            _viewModel = new ExaminationRecommenderDialogPatientViewModel(patient);
            DataContext = _viewModel;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void BtnSelect_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.SelectCommand.Execute(null);
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
