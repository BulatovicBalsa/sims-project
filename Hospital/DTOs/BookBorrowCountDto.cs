using Hospital.Models.Books;

namespace Hospital.DTOs;

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