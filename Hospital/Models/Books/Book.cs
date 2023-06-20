using System;
using System.Collections.Generic;

namespace Hospital.Models.Books;

public enum BindingType
{
    Hardcover,
    Paperback
}

public class Book
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Isbn { get; set; }
    public List<int> Udc { get; set; }
    public BindingType BindingType { get; set; }

    public Book()
    {
        Id = Guid.NewGuid().ToString();
        Description = Isbn = Title = "";
        Udc = new List<int>();
    }

    public Book(string title, string description, string isbn, List<int> udc, BindingType bindingType)
    {
        Id = Guid.NewGuid().ToString();
        Title = title;
        Description = description;
        Isbn = isbn;
        Udc = udc;
        BindingType = bindingType;
    }
}