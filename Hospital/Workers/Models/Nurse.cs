using Hospital.Accounts.Models;

namespace Hospital.Workers.Models;

public class Nurse : Person
{
    public Nurse()
    {
    }

    public Nurse(string firstName, string lastName, string jmbg, string username, string password) : base(firstName,
        lastName, jmbg, username, password)
    {
    }
}