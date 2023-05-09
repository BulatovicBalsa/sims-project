namespace Hospital.Models;

public class Profile
{
    public Profile()
    {
        Username = "";
        Password = "";
    }

    public Profile(string username, string password)
    {
        Username = username;
        Password = password;
    }

    public string Username { get; set; }
    public string Password { get; set; }
}