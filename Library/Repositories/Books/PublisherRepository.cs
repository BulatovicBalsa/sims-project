using System.Collections.Generic;
using Library.Models.Books;
using Library.Serialization;

namespace Library.Repositories.Books
{
    public class PublisherRepository
    {
        private const string FilePath = "../../../Data/books.csv";
        private readonly ISerializer<Publisher> _serializer;

        public PublisherRepository()
        {
            _serializer = new CsvSerializer<Publisher>();
        }

        public List<Publisher> GetAll()
        {
            return _serializer.Load(FilePath);
        }

        public Publisher? GetById(string id)
        {
            return GetAll().Find(publisher => publisher.Id == id);
        }

        public void Add(Publisher publisher)
        {
            var allPublishers = GetAll();

            allPublishers.Add(publisher);

            _serializer.Save(allPublishers, FilePath);
        }

        public void Update(Publisher publisher)
        {
            var allPublishers = GetAll();

            var indexToUpdate = allPublishers.FindIndex(e => e.Id == publisher.Id);
            if (indexToUpdate == -1) throw new KeyNotFoundException();

            allPublishers[indexToUpdate] = publisher;

            _serializer.Save(allPublishers, FilePath);
        }

        public void Delete(Publisher publisher)
        {
            var allPublishers = GetAll();

            var indexToDelete = allPublishers.FindIndex(e => e.Id == publisher.Id);
            if (indexToDelete == -1) throw new KeyNotFoundException();

            allPublishers.RemoveAt(indexToDelete);

            _serializer.Save(allPublishers, FilePath);
        }

        public void DeleteAll()
        {
            var emptyPublisherList = new List<Publisher>();
            _serializer.Save(emptyPublisherList, FilePath);
        }
    }
}
