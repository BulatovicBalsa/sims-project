using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Library.Exceptions;
using Library.Models.Books;
using Library.Services.Books;
using Library.ViewModels.Books;
using Library.Views.Books;
using Library.Views.Members;

namespace Library.ViewModels.Members;

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

    public MemberViewModel(Models.Member member)
    {
        _member = member;
        Books = new ObservableCollection<Book>(_bookService.GetDistinct());
        _books = Books;
        Loans =
            new ObservableCollection<Loan>(_loanService.GetCurrentLoans(member));
        _loans = Loans;
        SearchBoxText = Placeholder;

        ViewAdvancedBookDetailsCommand = new RelayCommand<string>(ViewAdvancedBookDetails);
        AddLoanCommand = new RelayCommand(AddLoan);
        UpdateLoanCommand = new RelayCommand(UpdateLoan);
        DeleteLoanCommand = new RelayCommand(DeleteLoan);
        DefaultLoanViewCommand = new RelayCommand(DefaultLoanView);
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

    public object SelectedLoan { get; set; }

    public string DoctorName => $"Member {Member.FirstName} {Member.LastName}";

    public ICommand ViewAdvancedBookDetailsCommand { get; set; }
    public ICommand AddLoanCommand { get; set; }
    public ICommand UpdateLoanCommand { get; set; }
    public ICommand DeleteLoanCommand { get; set; }
    public ICommand DefaultLoanViewCommand { get; set; }
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

    private void DefaultLoanView()
    {
        Loans.Clear();
        _loanService.GetCurrentLoans(_member).ForEach(Loans.Add);
    }

    private void AddLoan()
    {
        var dialog = new ModifyExaminationDialog()
        {
            DataContext = new ModifyLoanViewModel(Member, Loans)
        };

        dialog.ShowDialog();
    }

    private void UpdateLoan()
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

    private void DeleteLoan()
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
        DefaultLoanView();
        MessageBox.Show("Loan successfully returned");
    }

}