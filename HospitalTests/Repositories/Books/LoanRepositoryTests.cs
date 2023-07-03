using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hospital.Repositories.Books;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Injectors;
using Hospital.Models.Books;
using Hospital.Serialization;

namespace Hospital.Repositories.Books.Tests
{
    [TestClass()]
    public class LoanRepositoryTests
    {
        [TestInitialize]
        public void SetUp()
        {
            try
            {
                DeleteData();
            }
            catch (Exception)
            {
                Console.WriteLine("Files don't exist.");
            }
        }

        [TestCleanup]
        public void CleanUp()
        {
            try
            {
                DeleteData();
            }
            catch (Exception )
            {
                Console.WriteLine("Files don't exist.");
            }
        }

        private static void DeleteData()
        {
            Directory.GetFiles("../../../Data/").ToList().ForEach(File.Delete);
        }

        [TestMethod()]
        public void GetBooksOrderedByBorrowCountTest()
        {
            var bookRepository = new BookRepository(SerializerInjector.CreateInstance<ISerializer<Book>>());
            var loanRepository = new LoanRepository(SerializerInjector.CreateInstance<ISerializer<Loan>>());

            var books = new List<Book>()
            {
                new Book("The Great Gatsby", "", "", new List<int>{1, 2}, BindingType.Hardcover, "", BookLanguage.English),
                new Book("The Greater Gatsby", "", "", new List<int>{1, 2}, BindingType.Hardcover, "",
                    BookLanguage.English),
                new Book("The Greatest Gatsby", "", "", new List<int>{1, 2}, BindingType.Hardcover, "",
                    BookLanguage.English)
            };


            foreach (var book in books)
            {
                bookRepository.Add(book);
            }

            var curDate = DateTime.Now;
            for (var i = 0; i < 10; i++)
            {
                loanRepository.Add(new Loan(books[i % 3], new Models.Doctor.Doctor(), curDate.AddDays(i * -1), DateTime.Now));
            }

            var topBooks = loanRepository.GetBooksOrderedByBorrowCount(curDate.AddDays(-3));
            Assert.AreEqual(books[0].Id, topBooks[0].Book.Id);
        }
    }
}