namespace Hospital.Models
{
    public class Profile
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public Profile(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}