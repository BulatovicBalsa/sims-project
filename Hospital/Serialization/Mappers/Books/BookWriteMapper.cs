using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper.Configuration;
using Hospital.Models.Books;

namespace Hospital.Serialization.Mappers.Books
{
    public sealed class BookWriteMapper : ClassMap<Book>
    {
        public BookWriteMapper()
        {
            Map(book => book.Id).Index(0);
            Map(book => book.Title).Index(1);
            Map(book => book.Author!.Id).Index(2); 
            Map(book => book.Description).Index(3);
            Map(book => book.Isbn).Index(4);
            Map(book => book.Udc).Index(5).Convert(row => string.Join(Book.UdcSeparator, row.Value.Udc));
            Map(book => book.BindingType).Index(6);
            Map(book => book.Language).Index(7);
            Map(book => book.Genre!.Id).Index(8);
        }
    }
}
