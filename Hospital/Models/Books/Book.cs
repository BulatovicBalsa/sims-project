﻿using System;
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
    public const string UdcSeparator = "|";

    public Book()
    {
        Id = Guid.NewGuid().ToString();
        Description = Isbn = Title = "";
        Udc = new List<int>();
        Author = "";
        Genre = "";
    }

    public Book(string title, string description, string isbn, List<int> udc, BindingType bindingType, string author,
        BookLanguage language, string genre)
    {
        Id = Guid.NewGuid().ToString();
        Title = title;
        Description = description;
        Isbn = isbn;
        Udc = udc;
        BindingType = bindingType;
        Author = author;
        Language = language;
        Genre = genre;
    }

    public string Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Isbn { get; set; }
    public List<int> Udc { get; set; }
    public BindingType BindingType { get; set; }
    public BookLanguage Language { get; set; }
    public string Author { get; set; } //change later, this is just temporary
    public string UdcAsString => string.Join(UdcSeparator, Udc.Select(u => u.ToString()));
    public string Genre { get; set; }

    public string ComboBoxString => $"{Title}, {Author}";

    public override string ToString()
    {
        return $"{Id},{Title},{Author},{Description},{Isbn},{UdcAsString.Replace("-", "|")},{BindingType},{Language},{Genre}";
    }

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