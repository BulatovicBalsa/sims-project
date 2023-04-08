namespace Hospital.Models
{
    public class Person
    {
        static int freeId = 0;
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string JMBG { get; set; }
        public Profile Profile { get; set; }
        public Person(string firstName, string lastName, string jmbg, string username, string password)
        {
            Id = freeId;
            freeId++;
            FirstName = firstName;
            LastName = lastName;
            JMBG = jmbg;
            Profile = new Profile(username, password);
        }

    }
}