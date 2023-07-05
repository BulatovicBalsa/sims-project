using System.Collections.ObjectModel;
using System.Windows.Input;
using Library.Models.Books;
using Library.Repositories.Books;
using Library.Serialization;
using Library.Views.Librarian;

namespace Library.ViewModels.Librarian;

public class BookGridViewModel : ViewModelBase
{
    private readonly BookRepository _bookRepository;
    private ObservableCollection<Book> _books;
    private Book? _selectedBook;

    public BookGridViewModel()
    {
        _bookRepository = new BookRepository(new CsvSerializer<Book>());
        _books = new ObservableCollection<Book>(_bookRepository.GetAll());

        _bookRepository.BookAdded += Book =>
        {
            _books.Add(Book);
        };

        _bookRepository.BookUpdated += Book =>
        {
            _books.Remove(Book);
            _books.Add(Book);
        };

        AddBookCommand = new ViewModelCommand(ExecuteAddBookCommand);
        UpdateBookCommand = new ViewModelCommand(ExecuteUpdateBookCommand, IsBookSelected);
        DeleteBookCommand = new ViewModelCommand(ExecuteDeleteBookCommand, IsBookSelected);
    }

    public Book? SelectedBook
    {
        get => _selectedBook;
        set
        {
            _selectedBook = value;
            OnPropertyChanged(nameof(SelectedBook));
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

    public ICommand AddBookCommand { get; }
    public ICommand UpdateBookCommand { get; }
    public ICommand DeleteBookCommand { get; }
    private void ExecuteAddBookCommand(object obj)
    {
        var addPatientDialog = new AddBookView
        {
            DataContext = new AddUpdateBookViewModel(_bookRepository)
        };

        addPatientDialog.ShowDialog();
    }

    private void ExecuteUpdateBookCommand(object obj)
    {
        var updatePatientDialog = new UpdateBookView()
        {
            DataContext = new AddUpdateBookViewModel(_bookRepository, SelectedBook)
        };

        updatePatientDialog.ShowDialog();
    }

    private void ExecuteDeleteBookCommand(object obj)
    {
        _bookRepository.Delete(SelectedBook);
        _books.Remove(SelectedBook);
        SelectedBook = null;
    }

    private bool IsBookSelected(object obj)
    {
        return _selectedBook != null;
    }
}