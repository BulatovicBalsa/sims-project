using System;

namespace Library.Models.Memberships;

public class Membership
{
    public Membership(string name, int bookLimit, int loanDaysLimit, double price)
    {
        TypeId = Guid.NewGuid().ToString();
        Name = name;
        BookLimit = bookLimit;
        LoanDaysLimit = loanDaysLimit;
        Price = price;
    }

    public Membership()
    {
        TypeId = Guid.NewGuid().ToString();
        Name = "";
    }

    public string TypeId { get; set; }
    public string Name { get; set; }
    public int BookLimit { get; set; }
    public int LoanDaysLimit { get; set; }
    public double Price { get; set; }

    public override string ToString()
    {
        return Name;
    }
}