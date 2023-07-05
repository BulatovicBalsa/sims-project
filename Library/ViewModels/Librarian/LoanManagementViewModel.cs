using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using Library.Exceptions;
using Library.Models;
using Library.Models.Books;
using Library.Services.Books;
using Library.ViewModels.Books;
using Library.ViewModels.Members;
using Library.Views.Books;
using Library.Views.Members;

namespace Library.ViewModels.Librarian
{
    public class LoanManagementViewModel : ViewModelBase
    {
        private readonly LoanService _loanService = new();
        private ObservableCollection<Loan> _loans;

        public LoanManagementViewModel()
        {
            Loans =
                new ObservableCollection<Loan>(_loanService.GetCurrentLoans());
            _loans = Loans;

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

        public object SelectedLoan { get; set; }

        public ICommand AddLoanCommand { get; set; }
        public ICommand UpdateLoanCommand { get; set; }
        public ICommand DeleteLoanCommand { get; set; }
        public ICommand DefaultLoanViewCommand { get; set; }
        public ICommand ViewMostBorrowedBooksCommand { get; set; }
        public ICommand ReturnLoanCommand { get; set; }

        private void DefaultLoanView()
        {
            Loans.Clear();
            _loanService.GetCurrentLoans().ForEach(Loans.Add);
        }

        private void AddLoan()
        {
            var dialog = new ModifyExaminationDialog()
            {
                DataContext = new ModifyLoanViewModel(null, Loans)
            };

            dialog.ShowDialog();
        }

        private void UpdateLoan()
        {
            if (SelectedLoan is not Loan loanToChange)
            {
                MessageBox.Show("Please select examination in order to delete it");
                return;
            }

            var dialog = new ModifyExaminationDialog()
            {
                DataContext = new ModifyLoanViewModel(loanToChange.Member, Loans, loanToChange)
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
}
