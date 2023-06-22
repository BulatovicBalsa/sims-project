
using Hospital.Models.Books;

namespace Hospital.ViewModels.Books
{
    public class AdvancedBookDetailsViewModel : ViewModelBase
    {
        public Book Book { get; set; }

        public AdvancedBookDetailsViewModel(Book book)
        {
            Book = book;
        }


    }
}
