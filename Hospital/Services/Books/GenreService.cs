using Hospital.Injectors;
using Hospital.Models.Books;
using Hospital.Repositories.Books;
using Hospital.Serialization;
using System.Collections.Generic;

namespace Hospital.Services.Books;

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