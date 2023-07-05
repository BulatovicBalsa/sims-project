using System.Collections.Generic;
using Library.Injectors;
using Library.Models.Books;
using Library.Repositories.Books;
using Library.Serialization;

namespace Library.Services.Books;

public class GenreService
{
    private readonly GenreRepository
        _genreRepository = new(SerializerInjector.CreateInstance<ISerializer<Genre>>());

    public List<Genre> GetAll()
    {
        return _genreRepository.GetAll();
    }

    public Genre? GetById(string id)
    {
        return _genreRepository.GetById(id);
    }
}