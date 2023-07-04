using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Hospital.Models;
using Hospital.ViewModels;

namespace Hospital.Views;

public partial class DoctorView : Window
{
    private readonly string placeholder = "Search...";
    private readonly MemberViewModel _viewModel;
    private bool isUserInput = true;

    public DoctorView(Member member)
    {
        isUserInput = false;

        InitializeComponent();

        _viewModel = new MemberViewModel(member);
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

        var filteredPatients = _viewModel.Books.Where(book =>
            book.Title.ToLower().Contains(searchText) ||
            book.Author!.ToString().ToLower().Contains(searchText) ||
            book.Language.ToString().ToLower().Contains(searchText) ||
            book.Genre.ToString().ToLower().Contains(searchText)).ToList();

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
        if (!string.IsNullOrEmpty(SearchBox.Text)) return;
        isUserInput = false;
        SearchBox.Text = placeholder;
        SearchBox.Foreground = Brushes.Gray;
    }
}