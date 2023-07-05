using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Library.Injectors;
using Library.Models.Books;
using Library.Repositories.Books;
using Library.Serialization;
using Library.Services.Books;

namespace Library.ViewModels.Books;

public class MostBorrowedBooksViewModel : ViewModelBase
{
    private const int BookCount = 10;
    private ObservableCollection<Book> _books;
    private DateTime _from;

    public MostBorrowedBooksViewModel()
    {
        From = DateTime.Now.AddMonths(-1);
        var loanRepository = new LoanRepository(SerializerInjector.CreateInstance<ISerializer<Loan>>());
        Books = new ObservableCollection<Book>(loanRepository.GetBooksOrderedByBorrowCount(From).Take(BookCount)
            .Select(e => e.Book)
            .ToList());
        ViewAdvancedBookDetailsCommand = new RelayCommand<string>(ViewAdvancedBookDetails);
    }

    public DateTime From
    {
        get => _from;
        set
        {
            if (value.Equals(_from)) return;
            _from = value;
            OnPropertyChanged(nameof(From));
        }
    }

    public ObservableCollection<Book> Books
    {
        get => _books;
        set
        {
            if (Equals(value, _books)) return;
            _books = value;
            OnPropertyChanged(nameof(Books));
        }
    }

    public ICommand ViewAdvancedBookDetailsCommand { get; set; }

    private void ViewAdvancedBookDetails(string bookId)
    {
        var book = new BookService().GetBookById(bookId);
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
}