using System;
using System.Collections.Generic;
using System.Linq;

namespace Hospital.Models.Books;

public enum BindingType
{
    Hardcover,
    Paperback
}

public enum BookLanguage
{
    English,
    French,
    Spanish,
    Russian,
    Italian,
    German
}

public class Book
{
    public Book()
    {
        Id = Guid.NewGuid().ToString();
        Description = Isbn = Title = "";
        Udc = new List<int>();
        Author = "";
    }

    public Book(string title, string description, string isbn, List<int> udc, BindingType bindingType, string author, BookLanguage language)
    {
        Id = Guid.NewGuid().ToString();
        Title = title;
        Description = description;
        Isbn = isbn;
        Udc = udc;
        BindingType = bindingType;
        Author = author;
        Language = language;
    }

    public string Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Isbn { get; set; }
    public List<int> Udc { get; set; }
    public BindingType BindingType { get; set; }
    public BookLanguage Language { get; set; }
    public string Author { get; set; } //change later, this is just temporary
    public string UdcAsString => string.Join("-", Udc.Select(u => u.ToString()));

    public override string ToString()
    {
        return $"{Id},{Title},{Author},{Description},{Isbn},{UdcAsString.Replace("-", "|")},{BindingType},{Language}";
    }

    public string ComboBoxString => $"{Title}, {Author}";

    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType()) return false;

        if (obj is not Book other) return false;
        return Id == other.Id;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}