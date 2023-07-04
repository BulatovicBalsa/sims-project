using System;

namespace Hospital.Models
{
    public enum UserType
    {
        Administrator,
        SeniorLibrarian,
        Librarian,
        Member
    }
    public class User
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string JMBG { get; set; }
        public UserType Type { get; set; }
        public Profile Profile { get; set; }

        public User()
        {
            
        }

        public User(string firstName, string lastName, DateTime birthDate, string email, string phoneNumber,
            string jmbg, UserType type, string username, string password)
        {
            Id = Guid.NewGuid().ToString();
            FirstName = firstName;
            LastName = lastName;
            BirthDate = birthDate;
            Email = email;
            PhoneNumber = phoneNumber;
            JMBG = jmbg;
            Type = type;
            Profile = new Profile(username, password);
        }

        public override bool Equals(object? obj)
        {
            if (obj is not User objAsUser) return false;
            return Id == objAsUser.Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

    }
}
