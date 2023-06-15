using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Hospital.Core.Workers.Models;
using Hospital.GUI.ViewModels.Workers;

namespace Hospital.GUI.Views;

public partial class DoctorView : Window
{
    private readonly string placeholder = "Search...";
    private readonly DoctorViewModel _viewModel;
    private bool isUserInput = true;

    public DoctorView(Doctor doctor)
    {
        isUserInput = false;

        InitializeComponent();

        _viewModel = new DoctorViewModel(doctor);
        ConfigWindow();
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
        var searchText = SearchBox.Text.ToLower();

        var filteredPatients = _viewModel.Patients.Where(patient =>
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