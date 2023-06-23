using System;
using System.Collections.Generic;
using System.Linq;
using CsvHelper.Configuration;
using Hospital.Models.Books;

namespace Hospital.Serialization.Mappers.Books
{
    public sealed class BookReadMapper : ClassMap<Book>
    {
        public BookReadMapper()
        {
            Map(book => book.Id).Index(0);
            Map(book => book.Title).Index(1);
            Map(book => book.Author).Index(2); // edit later
            Map(book => book.Description).Index(3);
            Map(book => book.Isbn).Index(4);
            Map(book => book.Udc).Index(5).Convert(row => SplitColumnValues(row.Row.GetField<string>("Udc")).Select(udcPart => Convert.ToInt32(udcPart)).ToList());
            Map(book => book.BindingType).Index(6);
            Map(book => book.Language).Index(7);
        }

        private static List<string> SplitColumnValues(string? columnValue)
        {
            return columnValue?.Split("|").ToList() ?? new List<string>();
        }
    }
}
