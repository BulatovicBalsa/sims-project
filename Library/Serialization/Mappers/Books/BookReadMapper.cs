using System;
using System.Collections.Generic;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using Library.Injectors;
using Library.Models.Books;
using Library.Repositories.Books;

namespace Library.Serialization.Mappers.Books
{
    public sealed class BookReadMapper : ClassMap<Book>
    {
        public BookReadMapper()
        {
            Map(book => book.Id).Index(0);
            Map(book => book.Title).Index(1);
            Map(book => book.Author).Index(2).TypeConverter<AuthorTypeConverter>();
            Map(book => book.Description).Index(3);
            Map(book => book.Isbn).Index(4);
            Map(book => book.Udc).Index(5).Convert(row => SplitColumnValues(row.Row.GetField<string>(5)).Select(udcPart => Convert.ToInt32(udcPart)).ToList());
            Map(book => book.BindingType).Index(6);
            Map(book => book.Language).Index(7);
            Map(book => book.Genre).Index(8).TypeConverter<GenreTypeConverter>();
        }

        public class GenreTypeConverter : DefaultTypeConverter
        {
            public override object? ConvertFromString(string? text, IReaderRow row, MemberMapData memberMapData)
            {
                var genreId = text?.Trim() ?? "";
                var genre = new GenreRepository(SerializerInjector.CreateInstance<ISerializer<Genre>>()).GetById(genreId);
                return genre;
            }
        }

        public class AuthorTypeConverter : DefaultTypeConverter
        {
            public override object? ConvertFromString(string? text, IReaderRow row, MemberMapData memberMapData)
            {
                var authorId = text?.Trim() ?? "";
                var author = new AuthorRepository().GetById(authorId);
                return author;
            }
        }

        private static List<string> SplitColumnValues(string? columnValue)
        {
            return columnValue?.Split(Book.UdcSeparator).ToList() ?? new List<string>();
        }
    }
}
