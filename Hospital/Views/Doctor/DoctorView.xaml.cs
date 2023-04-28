using GalaSoft.MvvmLight;
using Hospital.Coordinators;
using Hospital.Exceptions;
using Hospital.Models.Doctor;
using Hospital.Models.Examination;
using Hospital.Models.Patient;
using Hospital.Repositories.Doctor;
using Hospital.Repositories.Examinaton;
using Hospital.Repositories.Patient;
using Hospital.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public partial class DoctorView : Window
    {
        private bool isUserInput = true;
        private readonly string placeholder = "Search...";
        private DoctorViewModel _viewModel; 

        public DoctorView(Doctor doctor)
        {
            isUserInput = false;
            _viewModel = new DoctorViewModel(doctor);

            ConfigWindow();

            InitializeComponent();
        }

        private void ConfigWindow()
        {
            DataContext = _viewModel;
            SizeToContent = SizeToContent.Height;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!isUserInput)
            {
                isUserInput = true;
                return;
            }
            SearchBox.Foreground = Brushes.Black;
            string searchText = SearchBox.Text.ToLower();

            List<Patient> filteredPatients = _viewModel.Patients.Where(patient =>
                patient.FirstName.ToLower().Contains(searchText) ||
                patient.LastName.ToLower().Contains(searchText) ||
                patient.Jmbg.ToLower().ToLower().Contains(searchText) ||
                patient.Id.ToLower().Contains(searchText)).ToList();

            PatientsDataGrid.ItemsSource = filteredPatients;
        }

        private void SearchBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (SearchBox.Text == placeholder)
            {
                isUserInput = false;
                SearchBox.Text = "";
            }
        }

        private void SearchBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(SearchBox.Text))
            {
                isUserInput = false;
                SearchBox.Text = placeholder;
                SearchBox.Foreground = Brushes.Gray;
            }
        }
    }
}
