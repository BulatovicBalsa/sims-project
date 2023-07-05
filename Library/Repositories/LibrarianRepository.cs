using System.Collections.Generic;
using System.Linq;
using Library.Exceptions;
using Library.Models;
using Library.Serialization;

namespace Library.Repositories;

public class LibrarianRepository
{
    private const string FilePath = "../../../Data/librarians.csv";
    private static LibrarianRepository? _instance;
    public static LibrarianRepository Instance => _instance ??= new LibrarianRepository();
    private LibrarianRepository() { }
    public List<Librarian> GetAll()
    {
        return CsvSerializer<Librarian>.FromCSV(FilePath);
    }

    public Librarian? GetById(string id)
    {
        return GetAll().FirstOrDefault(librarian => librarian.Id == id);
    }

    public Librarian? GetByUsername(string username)
    {
        return GetAll().FirstOrDefault(librarian => librarian.Profile.Username == username);
    }

    public void Add(Librarian librarian)
    {
        var allLibrarians = GetAll();
        allLibrarians.Add(librarian);
        CsvSerializer<Librarian>.ToCSV(allLibrarians, FilePath);
    }
    public void Update(Librarian librarian)

    {
        var allLibrarians = GetAll();

        var indexToUpdate = allLibrarians.FindIndex(librarianRecord => librarianRecord.Id == librarian.Id);
        if (indexToUpdate == -1)
            throw new ObjectNotFoundException($"Librarian with id {librarian.Id} was not found.");
        allLibrarians[indexToUpdate] = librarian;

        CsvSerializer<Librarian>.ToCSV(allLibrarians, FilePath);
    }

    public void Delete(Librarian librarian)
    {
        var allLibrarians = GetAll();

        if (!allLibrarians.Remove(librarian))
            throw new ObjectNotFoundException($"Librarian with id {librarian.Id} was not found.");

        CsvSerializer<Librarian>.ToCSV(allLibrarians, FilePath);
    }
}