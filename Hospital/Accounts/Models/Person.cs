using System;

namespace Hospital.Accounts.Models;

public abstract class Person
{
    protected Person()
    {
        Id = Guid.NewGuid().ToString();
        FirstName = "";
        LastName = "";
        Jmbg = "";
        Profile = new Profile();
    }

    protected Person(string firstName, string lastName, string jmbg, string username, string password)
    {
        Id = Guid.NewGuid().ToString();
        FirstName = firstName;
        LastName = lastName;
        Jmbg = jmbg;
        Profile = new Profile(username, password);
    }

    public string Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Jmbg { get; set; }
    public Profile Profile { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj is not Person objAsPerson) return false;
        return Id == objAsPerson.Id;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public override string ToString()
    {
        return $"{FirstName} {LastName}, {Jmbg}";
    }
}