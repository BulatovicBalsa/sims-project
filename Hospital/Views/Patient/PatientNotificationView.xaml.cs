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
    /// Interaction logic for PatientNotificationView.xaml
    /// </summary>
    public partial class PatientNotificationView : Window
    {
        private PatientNotificationViewModel _viewModel;
        public PatientNotificationView(Patient patient)
        {
            InitializeComponent();
            _viewModel = new PatientNotificationViewModel(patient);
            DataContext = _viewModel;
        }

        private void BtnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.CreateNotification();
            Close();
        }
    }
}
