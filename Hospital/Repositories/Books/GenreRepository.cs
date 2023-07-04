using Hospital.Models.Books;
using Hospital.Serialization;
using System;
using System.Collections.Generic;

namespace Hospital.Repositories.Books;

public class GenreRepository
{
    private const string FilePath = "../../../Data/genres.csv";
    private readonly ISerializer<Genre> _serializer;

    public GenreRepository(ISerializer<Genre> serializer)
    {
        _serializer = serializer;
    }

    public event Action<Genre>? GenreAdded;
    public event Action<Genre>? GenreUpdated;

    public List<Genre> GetAll()
    {
        return _serializer.Load(FilePath);
    }

    public Genre? GetById(string id)
    {
        return GetAll().Find(genre => genre.Id == id);
    }

    public void Add(Genre genre)
    {
        var allGenre = GetAll();

        allGenre.Add(genre);

        _serializer.Save(allGenre, FilePath);
        GenreAdded?.Invoke(genre);
    }

    public void Update(Genre genre)
    {
        var allGenre = GetAll();

        var indexToUpdate = allGenre.FindIndex(e => e.Id == genre.Id);
        if (indexToUpdate == -1) throw new KeyNotFoundException();

        allGenre[indexToUpdate] = genre;

        _serializer.Save(allGenre, FilePath);
        GenreUpdated?.Invoke(genre);
    }

    public void Delete(Genre genre)
    {
        var allGenre = GetAll();

        var indexToDelete = allGenre.FindIndex(e => e.Id == genre.Id);
        if (indexToDelete == -1) throw new KeyNotFoundException();

        allGenre.RemoveAt(indexToDelete);

        _serializer.Save(allGenre, FilePath);
    }

    public void DeleteAll()
    {
        var emptyGenreList = new List<Genre>();
        _serializer.Save(emptyGenreList, FilePath);
    }
}