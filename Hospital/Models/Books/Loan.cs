using System;
namespace Hospital.Models.Books;
using Doctor;

public class Loan
{
    public string Id { get; set; }
    public Book Book { get; set; }
    public Doctor Member { get; set; }
    public DateTime Start { get; set; }
    public DateTime? End { get; set; }

    public Loan()
    {
        Id = Guid.NewGuid().ToString();
    }

    public Loan(string id, Book book, Doctor member, DateTime start, DateTime? end)
    {
        Id = Guid.NewGuid().ToString();
        Book = book;
        Member = member;
        Start = start;
        End = end;
    }

    public override string ToString()
    {
        return $"{Id},{Book.Id},{Member.Id},{Start},{End}";
    }
}