namespace Hospital.Models.Doctor;

public class Member : Person
{
    public Member(string firstName, string lastName, string jmbg, string username, string password,
        string specialization) : base(firstName, lastName, jmbg, username, password)
    {
        Specialization = specialization;
    }

    public Member()
    {
        Specialization = "Unknown";
    }

    public string Specialization { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType()) return false;

        var other = obj as Member;
        if (other == null) return false;
        return Id == other.Id;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public Member DeepCopy()
    {
        var copy = new Member(FirstName, LastName, Jmbg, Profile.Username, Profile.Password, Specialization)
        {
            Id = Id
        };

        return copy;
    }
}