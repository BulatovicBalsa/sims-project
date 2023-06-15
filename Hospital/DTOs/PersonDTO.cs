namespace Hospital.DTOs;

public enum Role
{
    Nurse,
    Doctor
}

public class PersonDTO
{
    public PersonDTO()
    {
    }

    public PersonDTO(string id, string firstName, string lastName, Role role)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Role = role;
    }

    public string Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public Role Role { get; set; }

    public override string ToString()
    {
        return $"{FirstName} {LastName}";
    }
}