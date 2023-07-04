using System;
using System.Collections.Generic;
using System.Linq;
using Hospital.Models;
using Hospital.Models.Books;
using Hospital.Serialization;
using Hospital.Serialization.Mappers.Books;

namespace Hospital.Repositories.Books;

public class BookRepository
{
    private const string FilePath = "../../../Data/books.csv";
    private readonly ISerializer<Book> _serializer;

    public event Action<Book>? BookAdded;
    public event Action<Book>? BookUpdated;

    public BookRepository(ISerializer<Book> serializer)
    {
        _serializer = serializer;
    }

    public List<Book> GetAll()
    {
        return _serializer.Load(FilePath, new BookReadMapper());
    }

    public Book? GetById(string id)
    {
        return GetAll().Find(book => book.Id == id);
    }

    public Book? GetByIsbn(string isbn)
    {
        return GetAll().Find(book => book.Isbn == isbn);
    }

    public List<Book> GetByUdc(List<int> udc)
    {
        return GetAll().FindAll(book => book.Udc.SequenceEqual(udc));
    }

    public void Add(Book book)
    {
        var allBook = GetAll();

        allBook.Add(book);

        _serializer.Save(allBook, FilePath, new BookWriteMapper());
        BookAdded?.Invoke(book);
    }

    public void Update(Book book)
    {
        var allBook = GetAll();

        var indexToUpdate = allBook.FindIndex(e => e.Id == book.Id);
        if (indexToUpdate == -1) throw new KeyNotFoundException();

        allBook[indexToUpdate] = book;

        _serializer.Save(allBook, FilePath);
        BookUpdated?.Invoke(book);
    }

    public void Delete(Book book)
    {
        var allBook = GetAll();

        var indexToDelete = allBook.FindIndex(e => e.Id == book.Id);
        if (indexToDelete == -1) throw new KeyNotFoundException();

        allBook.RemoveAt(indexToDelete);

        _serializer.Save(allBook, FilePath, new BookWriteMapper());
    }

    public void DeleteAll()
    {
        var emptyBookList = new List<Book>();
        _serializer.Save(emptyBookList, FilePath);
    }
}
