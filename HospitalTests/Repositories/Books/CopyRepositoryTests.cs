using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hospital.Repositories.Copys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Models.Books;
using Hospital.Serialization;

namespace Hospital.Repositories.Copys.Tests
{
    [TestClass()]
    public class CopyRepositoryTests
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
            catch (Exception)
            {
                Console.WriteLine("Files don't exist.");
            }
        }

        private static void DeleteData()
        {
            Directory.GetFiles("../../../Data/").ToList().ForEach(File.Delete);
        }
        [TestMethod()]
        public void TestAdd()
        {
            var book = new Book("Book", "", "1234567", new List<int>(), BindingType.Hardcover, "Someone Somewhere", BookLanguage.English);
            var copyRepository = new CopyRepository(new JsonSerializer<Copy>());
            copyRepository.Add(new Copy("1", book, 1000));
            copyRepository.Add(new Copy("2", book, 2000));
            var copies = copyRepository.GetAll();
            Assert.AreEqual(2, copies.Count);
            Assert.AreEqual(2000, copies[1].Price);
            Assert.AreEqual("Book", copies[1].Book.Title);
        }
    }
}