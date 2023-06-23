using GalaSoft.MvvmLight.Command;
using Hospital.Exceptions;
using Hospital.Models.Doctor;
using Hospital.Models.Examination;
using Hospital.Models.Manager;
using Hospital.Models.Patient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Hospital.Models.Books;
using Hospital.Services;
using Hospital.Services.Books;
using Hospital.Services.Manager;

namespace Hospital.ViewModels;

public class ModifyExaminationViewModel : ViewModelBase
{
    private readonly Doctor _member;
    private readonly ObservableCollection<Loan> _loanCollection;
    private readonly LoanService _loanService = new();
    private readonly Loan? _loanToChange;
    private readonly bool _isUpdate;
    private readonly BookService _bookService = new();
    private readonly DoctorService _doctorService = new();

    private string _buttonContent;

    private ObservableCollection<Book> _books;
    private ObservableCollection<Doctor> _members;
    private Book? _selectedBook;
    private Doctor? _selectedMember;

    public ModifyExaminationViewModel()
    {
        
    }
    public ModifyExaminationViewModel(Doctor member, ObservableCollection<Loan> loanCollection, Loan? loanToChange = null)
    {
        _isUpdate = loanToChange is not null;
        _member = member;
        _loanCollection = loanCollection;
        _loanToChange = loanToChange;

        _books = new ObservableCollection<Book>(_loanService.GetNotLoanedBooks());
        _members = new ObservableCollection<Doctor>(_doctorService.GetAll());

        _selectedBook = _loanToChange?.Book;
        _selectedMember = _loanToChange?.Member;
        _buttonContent = _loanToChange is null ? "Create Loan" : "Update Loan";

        ModifyLoanCommand = new RelayCommand(ModifyExamination);
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

    public ObservableCollection<Doctor> Members
    {
        get => _members;
        set
        {
            _members = value;
            OnPropertyChanged(nameof(Members));
        }
    }

    public string ButtonContent
    {
        get => _buttonContent;
        set
        {
            _buttonContent = value;
            OnPropertyChanged(nameof(ButtonContent));
        }
    }

    public Doctor? SelectedMember
    {
        get => _selectedMember;
        set
        {
            _selectedMember = value;
            OnPropertyChanged(nameof(SelectedMember));
        }
    }

    public ICommand ModifyLoanCommand { get; set; }

    private void ModifyExamination()
    {
        var createdLoan = CreateLoanFromForm();
        if (createdLoan is null) return;

        try
        {
            if (_isUpdate)
            {
                UpdateExamination(createdLoan);
            }
            else
            {
                _loanService.Add(createdLoan);
                _loanCollection.Add(createdLoan);
            }
        }
        catch (Exception ex)
        {
            if (ex is DoctorBusyException or BookAlreadyLoanedException or RoomBusyException)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }

        MessageBox.Show("Task successfully ended", "Good job", MessageBoxButton.OK, MessageBoxImage.Information);
        Application.Current.Windows[1]?.Close();
    }

    private Loan? CreateLoanFromForm()
    {
        if (SelectedBook == null
            || SelectedMember == null)
        {
            MessageBox.Show("You must select book and member");
            return null;
        }

        var startDate = DateTime.Today;

        var createdExamination = _isUpdate ? _loanToChange : new Loan();
        createdExamination?.Update(SelectedMember, SelectedBook, startDate, null);
        return createdExamination;
    }

    private void UpdateExamination(Loan loan)
    {
        if (_loanToChange != null) loan.Id = _loanToChange.Id;
        else throw new InvalidOperationException("loan to change can't be null");
        _loanService.Update(loan);
        _loanCollection.Clear();
        _loanService.GetCurrentLoans(_member)
            .ForEach(loanInRange => _loanCollection.Add(loanInRange));
    }
}