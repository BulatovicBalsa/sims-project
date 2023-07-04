using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Hospital.Models.Books;
using Hospital.Repositories.Books;
using Hospital.Serialization;

namespace Hospital.ViewModels.Librarian;

public class AddUpdateCopyViewModel : ViewModelBase
{
    private Book _book;
    private string _bookError;
    private ObservableCollection<Book> _books;
    private string _inventoryNumber;
    private string _inventoryNumberError;
    private string _price;
    private string _priceError;

    public AddUpdateCopyViewModel()
    {
        var bookRepository = new BookRepository(new CsvSerializer<Book>());
        Books = new ObservableCollection<Book>(bookRepository.GetAll());
        CancelCommand = new ViewModelCommand(ExecuteCancelCommand);
        AddCopyCommand = new ViewModelCommand(ExecuteAddCopyCommand);
    }

    public string InventoryNumber
    {
        get => _inventoryNumber;
        set
        {
            if (value == _inventoryNumber) return;
            _inventoryNumber = value;
            OnPropertyChanged(nameof(InventoryNumber));
        }
    }

    public string InventoryNumberError
    {
        get => _inventoryNumberError;
        set
        {
            if (value == _inventoryNumberError) return;
            _inventoryNumberError = value;
            OnPropertyChanged(nameof(InventoryNumberError));
        }
    }

    public Book Book
    {
        get => _book;
        set
        {
            if (Equals(value, _book)) return;
            _book = value;
            OnPropertyChanged(nameof(Book));
        }
    }

    public string BookError
    {
        get => _bookError;
        set
        {
            if (value == _bookError) return;
            _bookError = value;
            OnPropertyChanged(nameof(BookError));
        }
    }

    public string Price
    {
        get => _price;
        set
        {
            if (value == _price) return;
            _price = value;
            OnPropertyChanged(nameof(Price));
        }
    }

    public string PriceError
    {
        get => _priceError;
        set
        {
            if (value == _priceError) return;
            _priceError = value;
            OnPropertyChanged(nameof(PriceError));
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

    public event Action? DialogClosed;
    public ICommand AddCopyCommand { get; }
    public ICommand UpdateCopyCommand { get; }

    public ICommand CancelCommand { get; }

    private void ExecuteCancelCommand(object obj)
    {
        CloseDialog();
    }

    private void CloseDialog()
    {
        Application.Current.Windows[2]?.Close();
        DialogClosed?.Invoke();
    }

    private bool CheckInputErrors()
    {
        if (string.IsNullOrEmpty(InventoryNumber))
        {
            InventoryNumberError = "Enter inventory number";
            return false;
        }

        InventoryNumberError = "";

        if (Book == null)
        {
            BookError = "Select a book";
            return false;
        }

        BookError = "";

        double parsedPrice = -1.1;
        if (!double.TryParse(Price, out parsedPrice) || parsedPrice < 0)
        {
            PriceError = "Enter a non-negative price";
            return false;
        }

        return true;
    }

    private void ExecuteAddCopyCommand(object obj)
    {
        if (!CheckInputErrors())
            return;
        var copyRepository = new CopyRepository(new JsonSerializer<Copy>());
        var copy = new Copy(InventoryNumber, Book, double.Parse(Price));
        copyRepository.Add(copy);
        CloseDialog();
    }
}