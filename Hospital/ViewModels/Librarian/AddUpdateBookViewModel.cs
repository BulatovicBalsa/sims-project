﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Hospital.Models;
using Hospital.Models.Books;
using Hospital.Repositories;
using Hospital.Repositories.Books;
using Hospital.Serialization;

namespace Hospital.ViewModels.Librarian;

public class AddUpdateBookViewModel : ViewModelBase
{
    private readonly BookRepository _bookRepository;
    private readonly AuthorRepository _authorRepository;
    private readonly GenreRepository _genreRepository;
    private readonly Book? _bookToUpdate;
    private string _title;
    private string _description;
    private string _isbn;
    private string _udc;
    private Author? _author;
    private Genre? _genre;
    private ObservableCollection<BookLanguage> _allLanguages;
    private ObservableCollection<Author> _allAuthors;
    private ObservableCollection<Genre> _allGenres;
    private BookLanguage? _selectedLanguage;

    public event Action? DialogClosed;

    public AddUpdateBookViewModel()
    {
        // dummy constructor
    }

    public AddUpdateBookViewModel(BookRepository BookRepository)
    {
        _bookRepository = BookRepository;
        _genreRepository = new GenreRepository(new CsvSerializer<Genre>());
        _authorRepository = new AuthorRepository();
        
        _allLanguages = new ObservableCollection<BookLanguage>(Enum.GetValues<BookLanguage>());
        _allAuthors = new ObservableCollection<Author>(_authorRepository.GetAll());
        _allGenres = new ObservableCollection<Genre>(_genreRepository.GetAll());

        AddBookCommand = new ViewModelCommand(ExecuteAddBookCommand, CanExecuteAddUpdateBookCommand);
        CancelCommand = new ViewModelCommand(ExecuteCancelCommand);
    }

    public AddUpdateBookViewModel(BookRepository BookRepository, Book selectedBook)
    {
        _bookToUpdate = selectedBook;
        _bookRepository = BookRepository;
        _genreRepository = new GenreRepository(new CsvSerializer<Genre>());
        _authorRepository = new AuthorRepository();

        _allLanguages = new ObservableCollection<BookLanguage>(Enum.GetValues<BookLanguage>());
        _allAuthors = new ObservableCollection<Author>(_authorRepository.GetAll());
        _allGenres = new ObservableCollection<Genre>(_genreRepository.GetAll());

        SetViewModelProperties(selectedBook);

        UpdateBookCommand = new ViewModelCommand(ExecuteUpdateBookCommand, CanExecuteAddUpdateBookCommand);
        CancelCommand = new ViewModelCommand(ExecuteCancelCommand);
    }

    public string Title
    {
        get => _title;
        set
        {
            _title = value;
            OnPropertyChanged(nameof(Title));
        }
    }

    public string Description
    {
        get => _description;
        set
        {
            _description = value;
            OnPropertyChanged(nameof(Description));
        }
    }

    public string Isbn
    {
        get => _isbn;
        set
        {
            _isbn = value;
            OnPropertyChanged(nameof(Isbn));
        }
    }

    public string Udc
    {
        get => _udc;
        set
        {
            _udc = value;
            OnPropertyChanged(nameof(Udc));
        }
    }

    public Author? Author
    {
        get => _author;
        set
        {
            _author = value;
            OnPropertyChanged(nameof(Author));
        }
    }

    public Genre? Genre
    {
        get => _genre;
        set
        {
            _genre = value;
            OnPropertyChanged(nameof(Genre));
        }
    }

    public ObservableCollection<BookLanguage> AllLanguages
    {
        get => _allLanguages;
        set
        {
            _allLanguages = value;
            OnPropertyChanged(nameof(AllLanguages));
        }
    }

    public ObservableCollection<Genre> AllGenres
    {
        get => _allGenres;
        set
        {
            _allGenres = value;
            OnPropertyChanged(nameof(AllGenres));
        }
    }

    public ObservableCollection<Author> AllAuthors
    {
        get => _allAuthors;
        set
        {
            _allAuthors = value;
            OnPropertyChanged(nameof(AllAuthors));
        }
    }

    public BookLanguage? SelectedLanguage
    {
        get => _selectedLanguage;
        set
        {
            _selectedLanguage = value;
            OnPropertyChanged(nameof(SelectedLanguage));
        }
    }

    public ICommand AddBookCommand { get; }
    public ICommand UpdateBookCommand { get; }
    public ICommand CancelCommand { get; }

    private void SetViewModelProperties(Book selectedBook)
    {
        Title = selectedBook.Title;
        Description = selectedBook.Description;
        Isbn = selectedBook.Isbn;
        Udc = selectedBook.UdcAsString;
        SelectedLanguage = selectedBook.Language;
        Author = selectedBook.Author!;
        Genre = selectedBook.Genre!;
    }

    private void ExecuteAddBookCommand(object obj)
    {
        if (ErrorHappened())
            return;

        _bookRepository.Add(new Book(Title, Description, Isbn, Udc.Split(Book.UdcSeparator).Select(int.Parse).ToList(), BindingType.Paperback, Author, (BookLanguage)SelectedLanguage, Genre));

        CloseDialog();
    }

    private void ExecuteUpdateBookCommand(object obj)
    {
        if (ErrorHappened())
            return;

        SetBookFromProperties();

        _bookRepository.Update(_bookToUpdate);

        CloseDialog();
    }

    private void SetBookFromProperties()
    {
        _bookToUpdate.Title = Title;
        _bookToUpdate.Description = Description;
        _bookToUpdate.Isbn = Isbn;
        _bookToUpdate.Udc = Udc.Split(Book.UdcSeparator).Select(int.Parse).ToList();
        _bookToUpdate.Language = (BookLanguage)SelectedLanguage;
        _bookToUpdate.Author = Author;
        _bookToUpdate.Genre = Genre;
    }

    private void CloseDialog()
    {
        Application.Current.Windows[1]?.Close();
        DialogClosed?.Invoke();
    }

    private bool ErrorHappened()
    {
        return false;
    }

    private bool CanExecuteAddUpdateBookCommand(object obj)
    {
        var isAnyFieldNullOrEmpty = !string.IsNullOrEmpty(Title) &&
                                    !string.IsNullOrEmpty(Description) &&
                                    !string.IsNullOrEmpty(Isbn) &&
                                    !string.IsNullOrEmpty(Udc) &&
                                    Author is not null &&
                                    Genre is not null &&
                                    SelectedLanguage != null;

        return isAnyFieldNullOrEmpty;
    }

    private void ExecuteCancelCommand(object obj)
    {
        CloseDialog();
    }
}
