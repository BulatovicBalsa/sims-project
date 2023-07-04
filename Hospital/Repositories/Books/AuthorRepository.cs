using Hospital.Models.Books;
using Hospital.Serialization;
using System.Collections.Generic;

namespace Hospital.Repositories.Books
{
    public class AuthorRepository
    {
        private const string FilePath = "../../../Data/authors.csv";
        private readonly ISerializer<Author> _serializer;

        public AuthorRepository()
        {
            _serializer = new CsvSerializer<Author>();
        }

        public List<Author> GetAll()
        {
            return _serializer.Load(FilePath);
        }

        public Author? GetById(string id)
        {
            return GetAll().Find(author => author.Id == id);
        }

        public void Add(Author author)
        {
            var allAuthors = GetAll();

            allAuthors.Add(author);

            _serializer.Save(allAuthors, FilePath);
        }

        public void Update(Author author)
        {
            var allAuthors = GetAll();

            var indexToUpdate = allAuthors.FindIndex(e => e.Id == author.Id);
            if (indexToUpdate == -1) throw new KeyNotFoundException();

            allAuthors[indexToUpdate] = author;

            _serializer.Save(allAuthors, FilePath);
        }

        public void Delete(Author author)
        {
            var allAuthors = GetAll();

            var indexToDelete = allAuthors.FindIndex(e => e.Id == author.Id);
            if (indexToDelete == -1) throw new KeyNotFoundException();

            allAuthors.RemoveAt(indexToDelete);

            _serializer.Save(allAuthors, FilePath);
        }

        public void DeleteAll()
        {
            var emptyAuthorList = new List<Author>();
            _serializer.Save(emptyAuthorList, FilePath);
        }
    }
}
