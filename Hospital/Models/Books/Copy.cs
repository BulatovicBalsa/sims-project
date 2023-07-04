using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Hospital.Models.Books
{
    public enum CopyStatus
    {
        Available,
        Borrowed,
        Reserved,
        Damaged,
        Lost
    }
    public class Copy
    {
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

        public Loan Borrow(Doctor.Doctor member)
        {
            if (Status != CopyStatus.Available && Status != CopyStatus.Reserved)
                throw new InvalidOperationException();

            Status = CopyStatus.Borrowed;
            return new Loan(Book, member, DateTime.Now, null);
        }

        public bool IsAvailable()
        {
            return Status == CopyStatus.Reserved;
        }

        public void Return()
        {
            this.Status = CopyStatus.Available;
        }
    }
}
