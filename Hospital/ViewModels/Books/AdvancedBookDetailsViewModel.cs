using Hospital.Models.Books;
using System.Windows;
using System.Windows.Input;

namespace Hospital.ViewModels.Books
{
    public class AdvancedBookDetailsViewModel : ViewModelBase
    {
        public Book Book { get; set; }

        public AdvancedBookDetailsViewModel() { }
        public AdvancedBookDetailsViewModel(Book book)
        {
            Book = book;
            CloseDialogCommand = new ViewModelCommand(ExecuteCloseDialogCommand);
        }

        public ICommand CloseDialogCommand { get; }

        private void ExecuteCloseDialogCommand(object obj)
        {
            Application.Current.Windows[1]?.Close();
        }
    }
}
