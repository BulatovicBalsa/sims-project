using System;
namespace Hospital.Models.Books;

public class Loan
{
    public string Id { get; set; }
    public Book Book { get; set; }
    public Member Member { get; set; }
    public DateTime Start { get; set; }
    public DateTime? End { get; set; }

    public string InventoryNumber { get; set; }

    public Loan()
    {
        Id = Guid.NewGuid().ToString();
        Member = new Member();
        Book = new Book();
        InventoryNumber = "";
    }

    public Loan(Book book, Member member, DateTime start, DateTime? end, string inventoryNumber)
    {
        Id = Guid.NewGuid().ToString();
        Book = book;
        Member = member;
        Start = start;
        End = end;
        InventoryNumber = inventoryNumber;
    }

    public override string ToString()
    {
        return $"{Id},{Member.Id},{Book.Id},{Start},{End}";
    }

    public void Update(Member selectedMember, Book selectedBook, DateTime start, DateTime? end = null, string inventoryNumber = "")
    {
        Member = selectedMember;
        Book = selectedBook;
        Start = start;
        End = end;
        InventoryNumber = inventoryNumber;
    }

    public void Return()
    {
        End = DateTime.Now;
    }
}