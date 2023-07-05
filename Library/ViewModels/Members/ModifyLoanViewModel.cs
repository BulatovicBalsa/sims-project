using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Library.Exceptions;
using Library.Models.Books;
using Library.Services.Books;
using Library.Services.Members;

namespace Library.ViewModels.Members;

public class ModifyLoanViewModel : ViewModelBase
{
    private readonly Models.Member _member;
    private readonly ObservableCollection<Loan> _loanCollection;
    private readonly LoanService _loanService = new();
    private readonly Loan? _loanToChange;
    private readonly bool _isUpdate;
    private readonly MemberService _memberService = new();

    private string _buttonContent;

    private ObservableCollection<Book> _books;
    private ObservableCollection<Models.Member> _members;
    private Book? _selectedBook;
    private Models.Member? _selectedMember;
    private Copy? _selectedCopy;
    private ObservableCollection<Copy> _copies;
    private ICommand _modifyLoanCommand;

    public ModifyLoanViewModel() {}

    public ModifyLoanViewModel(Models.Member member, ObservableCollection<Loan> loanCollection, Loan? loanToChange = null)
    {
        _isUpdate = loanToChange is not null;
        _member = member;
        _loanCollection = loanCollection;
        _loanToChange = loanToChange;

        _books = new ObservableCollection<Book>(_loanService.GetNotLoanedBooks());
        _members = new ObservableCollection<Models.Member>(_memberService.GetAll());

        _selectedBook = _loanToChange?.Book;
        _selectedMember = _loanToChange?.Member;
        _buttonContent = _loanToChange is null ? "Create Loan" : "Update Loan";

        ModifyLoanCommand = new RelayCommand(ModifyLoan);
        Copies = new ObservableCollection<Copy>();
        SelectedCopy = null;
    }

    public Book? SelectedBook
    {
        get => _selectedBook;
        set
        {
            _selectedBook = value;
            RefreshCopyList();
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

    public ObservableCollection<Models.Member> Members
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

    public Models.Member? SelectedMember
    {
        get => _selectedMember;
        set
        {
            _selectedMember = value;
            OnPropertyChanged(nameof(SelectedMember));
        }
    }

    public ObservableCollection<Copy> Copies
    {
        get => _copies;
        set
        {
            if (Equals(value, _copies)) return;
            _copies = value;
            OnPropertyChanged(nameof(Copies));
        }
    }

    public Copy? SelectedCopy
    {
        get => _selectedCopy;
        set
        {
            if (Equals(value, _selectedCopy)) return;
            _selectedCopy = value;
            OnPropertyChanged(nameof(SelectedCopy));
        }
    }

    public ICommand ModifyLoanCommand
    {
        get => _modifyLoanCommand;
        set
        {
            if (Equals(value, _modifyLoanCommand)) return;
            _modifyLoanCommand = value;
            OnPropertyChanged(nameof(ModifyLoanCommand));
        }
    }

    private void ModifyLoan()
    {
        var createdLoan = CreateLoanFromForm();
        if (createdLoan is null) return;

        try
        {
            if (_isUpdate)
            {
                UpdateLoan(createdLoan);
            }
            else
            {
                _loanService.Add(createdLoan);
                _loanCollection.Clear();
                _loanService.GetCurrentLoans(_member)
                    .ForEach(loanInRange => _loanCollection.Add(loanInRange));
            }
        }
        catch (BookAlreadyLoanedException ex)
        {
                MessageBox.Show(ex.Message);
                return;
        }
        catch (MemberHasReachedMaxLoansException ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        MessageBox.Show("Task successfully ended", "Good job", MessageBoxButton.OK, MessageBoxImage.Information);
        Application.Current.Windows[1]?.Close();
    }

    private Loan? CreateLoanFromForm()
    {
        if (SelectedBook == null
            || SelectedMember == null || SelectedCopy == null)
        {
            MessageBox.Show("You must select book, member and copy");
            return null;
        }

        var startDate = DateTime.Today;

        var createdExamination = _isUpdate ? _loanToChange : new Loan();
        createdExamination?.Update(SelectedMember, SelectedBook, startDate, null, SelectedCopy.InventoryNumber);
        return createdExamination;
    }

    private void UpdateLoan(Loan loan)
    {
        if (_loanToChange != null) loan.Id = _loanToChange.Id;
        else throw new InvalidOperationException("loan to change can't be null");
        _loanService.Update(loan);
        _loanCollection.Clear();
        _loanService.GetCurrentLoans(_member)
            .ForEach(loanInRange => _loanCollection.Add(loanInRange));
    }

    private void RefreshCopyList()
    {
        Copies.Clear();
        _loanService.GetAvailableCopies(SelectedBook).ForEach(copy => Copies.Add(copy));
    }
}