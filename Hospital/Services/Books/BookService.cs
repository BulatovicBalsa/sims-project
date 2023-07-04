using System.Collections.Generic;
using System.Linq;
using Hospital.Injectors;
using Hospital.Models.Books;
using Hospital.Repositories.Books;
using Hospital.Serialization;

namespace Hospital.Services.Books;

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