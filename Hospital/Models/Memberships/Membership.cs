using System;

namespace Hospital.Models.Memberships;

public class Membership
{
    public Membership(string name, int bookLimit, int loanDaysLimit, double price)
    {
        Id = Guid.NewGuid().ToString();
        Name = name;
        BookLimit = bookLimit;
        LoanDaysLimit = loanDaysLimit;
        Price = price;
    }

    public Membership()
    {
        Id = Guid.NewGuid().ToString();
        Name = "";
    }

    public string Id { get; set; }
    public string Name { get; set; }
    public int BookLimit { get; set; }
    public int LoanDaysLimit { get; set; }
    public double Price { get; set; }
}