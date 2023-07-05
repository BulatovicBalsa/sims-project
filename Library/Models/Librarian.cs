namespace Library.Models;

public class Librarian : Person
{
    public Librarian()
    {
    }

    public Librarian(string firstName, string lastName, string jmbg, string username, string password) : base(firstName,
        lastName, jmbg, username, password)
    {
    }
}
