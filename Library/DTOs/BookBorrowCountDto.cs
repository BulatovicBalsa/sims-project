using Library.Models.Books;

namespace Library.DTOs;

public class BookBorrowCountDto
{
    public BookBorrowCountDto(Book book, int borrowCount)
    {
        Book = book;
        BorrowCount = borrowCount;
    }

    public Book Book { get; set; }
    public int BorrowCount { get; set; }
}