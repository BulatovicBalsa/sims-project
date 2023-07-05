using System.Collections.Generic;
using System.Linq;
using Library.Injectors;
using Library.Models.Books;
using Library.Repositories.Books;
using Library.Serialization;

namespace Library.Services.Books;

public class BookService
{
    private readonly BookRepository _bookRepository = new(SerializerInjector.CreateInstance<ISerializer<Book>>());

    public List<Book> GetAll()
    {
        return _bookRepository.GetAll();
    }

    public List<Book> GetDistinct()
    {
        return _bookRepository.GetAll().DistinctBy(book => book.Title).ToList();
    }

    public Book? GetBookById(string bookId) => _bookRepository.GetById(bookId);
}