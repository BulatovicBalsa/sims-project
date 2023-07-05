using System;
using Newtonsoft.Json;

namespace Library.Models.Books
{
    public enum CopyStatus
    {
        Available = 0,
        Borrowed = 1,
        Reserved = 2,
        Damaged = 3,
        Lost = 4
    }
    public class Copy
    {
        [JsonProperty]
        public CopyStatus Status { get; private set; }
        public string InventoryNumber { get; set; }
        public Book Book { get; set; }
        public double Price { get; set; }

        public Copy(string inventoryNumber, Book book, double price)
        {
            Status = CopyStatus.Available;
            InventoryNumber = inventoryNumber;
            Book = book;
            Price = price;
        }

        public Loan Borrow(Member member)
        {
            if (Status != CopyStatus.Available && Status != CopyStatus.Reserved)
                throw new InvalidOperationException();

            Status = CopyStatus.Borrowed;
            return new Loan(Book, member, DateTime.Now, null, InventoryNumber);
        }

        public bool IsAvailable()
        {
            return Status is CopyStatus.Reserved or CopyStatus.Available;
        }

        public void Return()
        {
            this.Status = CopyStatus.Available;
        }
    }
}
