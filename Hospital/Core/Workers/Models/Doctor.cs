using Hospital.Core.Accounts.Models;

namespace Hospital.Core.Workers.Models;

public class Doctor : Person
{
    public Doctor(string firstName, string lastName, string jmbg, string username, string password,
        string specialization) : base(firstName, lastName, jmbg, username, password)
    {
        Specialization = specialization;
    }

    public Doctor()
    {
        Specialization = "Unknown";
    }

    public string Specialization { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType()) return false;

        var other = obj as Doctor;
        if (other == null) return false;
        return Id == other.Id;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public Doctor DeepCopy()
    {
        var copy = new Doctor(FirstName, LastName, Jmbg, Profile.Username, Profile.Password, Specialization)
        {
            Id = Id
        };

        return copy;
    }
}