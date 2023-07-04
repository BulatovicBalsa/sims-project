using System;

namespace Hospital.Models.Books;

public class Genre
{
    public Genre()
    {
        Id = Name = "";
    }

    public Genre(string name)
    {
        Id = Guid.NewGuid().ToString();
        Name = name;
    }

    public string Id { get; set; }
    public string Name { get; set; }

    public override string ToString()
    {
        return $"{Name}";
    }
}