using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Hospital.Exceptions;
using Hospital.Models.Books;
using Hospital.Services.Books;
using Hospital.ViewModels.Books;
using Hospital.Views.Books;
using Hospital.Views.Members;

namespace Hospital.ViewModels.Members;

public class MemberViewModel : ViewModelBase
{
    private const string Placeholder = "Search...";
    private readonly BookService _bookService = new();
    private readonly LoanService _loanService = new();

    private ObservableCollection<Book> _books;

    private Models.Member _member;
    private ObservableCollection<Loan> _loans;

    private string _searchBoxText;

    private object? _selectedBook;

    private DateTime _selectedDate;

    public MemberViewModel(Models.Member member)
    {
        _member = member;
        _selectedDate = DateTime.Now;
        Books = new ObservableCollection<Book>(_bookService.GetDistinct());
        _books = Books;
        Loans =
            new ObservableCollection<Loan>(_loanService.GetCurrentLoans(member));
        _loans = Loans;
        SearchBoxText = Placeholder;

        ViewAdvancedBookDetailsCommand = new RelayCommand<string>(ViewAdvancedBookDetails);
        AddExaminationCommand = new RelayCommand(AddExamination);
        UpdateExaminationCommand = new RelayCommand(UpdateExamination);
        DeleteExaminationCommand = new RelayCommand(DeleteExamination);
        DefaultExaminationViewCommand = new RelayCommand(DefaultExaminationView);
        ViewMostBorrowedBooksCommand = new RelayCommand(ViewMostBorrowedBooks);
        ReturnLoanCommand = new RelayCommand(ReturnLoan);
    }

    public ObservableCollection<Loan> Loans
    {
        get => _loans;
        set
        {
            _loans = value;
            OnPropertyChanged(nameof(Loans));
        }
    }

    public ObservableCollection<Book> Books
    {
        get => _books;
        set
        {
            _books = value;
            OnPropertyChanged(nameof(Books));
        }
    }

    public Models.Member Member
    {
        get => _member;
        set
        {
            _member = value;
            OnPropertyChanged(nameof(Member));
        }
    }

    public string SearchBoxText
    {
        get => _searchBoxText;
        set
        {
            _searchBoxText = value;
            OnPropertyChanged(SearchBoxText);
        }
    }

    public object? SelectedBook
    {
        get => _selectedBook;
        set
        {
            _selectedBook = value;
            OnPropertyChanged(nameof(SelectedBook));
        }
    }

    public DateTime SelectedDate
    {
        get => _selectedDate;
        set
        {
            _selectedDate = value;
            OnPropertyChanged(nameof(SelectedDate));
            Loans.Clear();
            //_loanService.GetExaminationsForDate(_member, SelectedDate).ToList().ForEach(Loans.Add);
        }
    }

    public object SelectedLoan { get; set; }

    public string DoctorName => $"Member {Member.FirstName} {Member.LastName}";

    public ICommand ViewAdvancedBookDetailsCommand { get; set; }
    public ICommand AddExaminationCommand { get; set; }
    public ICommand UpdateExaminationCommand { get; set; }
    public ICommand DeleteExaminationCommand { get; set; }
    public ICommand DefaultExaminationViewCommand { get; set; }
    public ICommand ViewMostBorrowedBooksCommand { get; set; }
    public ICommand ReturnLoanCommand { get; set; }

    private void ViewAdvancedBookDetails(string bookId)
    {
        var book = _bookService.GetBookById(bookId);
        if (book == null)
        {
            MessageBox.Show("Select a book in order to see more details", "Error", MessageBoxButton.OK,
                MessageBoxImage.Error);
            return;
        }

        var dialog = new AdvancedBookDetailsDialog
        {
            DataContext = new AdvancedBookDetailsViewModel(book)
        };
        dialog.ShowDialog();
    }

    private void DefaultExaminationView()
    {
        Loans.Clear();
        _loanService.GetCurrentLoans(_member).ForEach(Loans.Add);
    }

    private void AddExamination()
    {
        var dialog = new ModifyExaminationDialog()
        {
            DataContext = new ModifyLoanViewModel(Member, Loans)
        };

        dialog.ShowDialog();
    }

    private void UpdateExamination()
    {
        if (SelectedLoan is not Loan examinationToChange)
        {
            MessageBox.Show("Please select examination in order to delete it");
            return;
        }

        var dialog = new ModifyExaminationDialog()
        {
            DataContext = new ModifyLoanViewModel(Member, Loans, examinationToChange)
        };

        dialog.ShowDialog();
    }

    private void DeleteExamination()
    {
        if (SelectedLoan is not Loan loan)
        {
            MessageBox.Show("Please select loan in order to delete it");
            return;
        }

        try
        {
            _loanService.DeleteLoan(loan);
        }
        catch (DoctorNotBusyException ex)
        {
            MessageBox.Show(ex.Message);
            return;
        }
        catch (BookNotLoanedException ex)
        {
            MessageBox.Show(ex.Message);
            return;
        }

        Loans.Remove(loan);
        MessageBox.Show("Succeed");
    }

    private void ViewMostBorrowedBooks()
    {
        var dialog = new MostBorrowedBooksDialog();
        Application.Current.Windows[0]!.Visibility = Visibility.Hidden;
        dialog.ShowDialog();
        Application.Current.Windows[0]!.Visibility = Visibility.Visible;
    }

    private void ReturnLoan()
    {
        var loan = SelectedLoan as Loan;
        if (loan == null)
        {
            MessageBox.Show("Please select a loan to return it");
            return;
        }
        _loanService.Return(loan);
        DefaultExaminationView();
        MessageBox.Show("Loan successfully returned");
    }

}