using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Library.Models.Books;

namespace Library.ViewModels.Books
{
    public class AdvancedBookDetailsViewModel : ViewModelBase
    {
        public Book Book { get; set; }

        public AdvancedBookDetailsViewModel() { }
        public AdvancedBookDetailsViewModel(Book book)
        {
            Book = book;
            CloseDialogCommand = new RelayCommand<Window>(ExecuteCloseDialogCommand);
        }

        public ICommand CloseDialogCommand { get; }

        private void ExecuteCloseDialogCommand(Window window)
        {
            window.DialogResult = true;
        }
    }
}
