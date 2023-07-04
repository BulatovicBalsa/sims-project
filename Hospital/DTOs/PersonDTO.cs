namespace Hospital.DTOs
{
    public enum Role
    {
        Librarian,
        Doctor
    }
    public class PersonDTO
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Role Role { get; set; }

        public PersonDTO() {}
        public PersonDTO(string id,string firstName, string lastName,Role role)
        {
            Id=id;
            FirstName=firstName;
            LastName=lastName;
            Role=role;
        }
        public override string ToString()
        {
            return $"{FirstName} {LastName}";
        }
    }



}
