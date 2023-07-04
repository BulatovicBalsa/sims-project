using System;
using System.Collections.Generic;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using Hospital.DTOs;
using Hospital.Exceptions;
using Hospital.Injectors;
using Hospital.Models.Books;
using Hospital.Repositories.Doctor;
using Hospital.Serialization;

namespace Hospital.Repositories.Books;
using Hospital.Models.Doctor;
public sealed class LoanReadMapper : ClassMap<Loan>
{
    public LoanReadMapper()
    {
        Map(loan => loan.Id).Index(0);
        Map(loan => loan.Member).Index(1).TypeConverter<DoctorTypeConverter>();
        Map(loan => loan.Book).Index(2).TypeConverter<BookTypeConverter>();
        Map(loan => loan.Start).Index(3);
        Map(loan => loan.End).Index(4);
        Map(loan => loan.InventoryNumber).Index(5);
    }

    public class DoctorTypeConverter : DefaultTypeConverter
    {
        public override object? ConvertFromString(string? inputText, IReaderRow rowData, MemberMapData mappingData)
        {
            var memberId = inputText?.Trim();
            if (string.IsNullOrEmpty(memberId))
                return null;
            
            var member =
                new DoctorRepository(SerializerInjector.CreateInstance<ISerializer<Doctor>>()).GetById(memberId) ??
                throw new KeyNotFoundException($"Member with ID {memberId} not found");
            return member;
        }
    }

    public class BookTypeConverter : DefaultTypeConverter
    {
        public override object ConvertFromString(string? inputText, IReaderRow rowData, MemberMapData mappingData)
        {
            var bookId = inputText?.Trim() ?? "";
            
            var book = new BookRepository(SerializerInjector.CreateInstance<ISerializer<Book>>()).GetById(bookId) ??
                          throw new KeyNotFoundException($"Book with ID {bookId} not found");
            return book;
        }
    }
}

public sealed class LoanWriteMapper : ClassMap<Loan>
{
    public LoanWriteMapper()
    {
        Map(loan => loan.Id).Index(0);
        Map(loan => loan.Member.Id).Index(1);
        Map(loan => loan.Book.Id).Index(2);
        Map(loan => loan.Start).Index(3);
        Map(loan => loan.End).Index(4);
        Map(loan => loan.InventoryNumber).Index(5);
    }
}

public class LoanRepository
{
    private const string FilePath = "../../../Data/loans.csv";
    private readonly ISerializer<Loan> _serializer;

    public LoanRepository(ISerializer<Loan> serializer)
    {
        _serializer = serializer;
    }

    public List<Loan> GetAll()
    {
        return _serializer.Load(FilePath, new LoanReadMapper());
    }

    public Loan? GetById(string id)
    {
        return GetAll().Find(loan => loan.Id == id);
    }

    public void Add(Loan loan)
    {
        var allLoans = GetAll();

        if (!IsFree(loan.InventoryNumber)) throw new BookAlreadyLoanedException("Book is already loaned");

        allLoans.Add(loan);

       _serializer.Save(allLoans, FilePath, new LoanWriteMapper());
    }

    public void Update(Loan loan)
    {
        var allLoans = GetAll();

        var indexToUpdate = allLoans.FindIndex(e => e.Id == loan.Id);
        if (indexToUpdate == -1) throw new KeyNotFoundException();

        allLoans[indexToUpdate] = loan;

       _serializer.Save(allLoans, FilePath, new LoanWriteMapper());
    }

    public void Delete(Loan loan)
    {
        var allLoan = GetAll();

        var indexToDelete = allLoan.FindIndex(e => e.Id == loan.Id);
        if (indexToDelete == -1) throw new KeyNotFoundException();

        allLoan.RemoveAt(indexToDelete);

       _serializer.Save(allLoan, FilePath, new LoanWriteMapper());
    }

    public List<Loan> GetAll(Doctor member)
    {
        var memberLoans = GetAll()
            .Where(loan => loan.Member.Equals(member))
            .ToList();
        return memberLoans;
    }

    public List<Loan> GetAll(Book book)
    {
        var bookLoans = GetAll()
            .Where(loan => loan.Book.Equals(book))
            .ToList();
        return bookLoans;
    }

    public List<Loan> GetFinishedLoans(Doctor member)
    {
        return GetAll(member).Where(loan => loan.End is not null).ToList();
    }

    public List<Loan> GetCurrentLoans(Doctor member)
    {
        return GetAll(member).Where(loan => loan.End is null).ToList();
    }

    public bool IsFree(string inventoryNumber)
    {
        var copyRepository = new CopyRepository(new JsonSerializer<Copy>());
        var copy = copyRepository.GetByInventoryNumber(inventoryNumber);
        return copy != null && copy.IsAvailable();
    }

    public void DeleteAll()
    {
        var emptyList = new List<Loan>();
        _serializer.Save(emptyList, FilePath, new LoanWriteMapper());
    }

    public List<Loan> GetLoans(DateTime startingFrom)
    {
        return GetAll().FindAll(e => e.Start >= startingFrom);
    }

    public List<BookBorrowCountDto> GetBooksOrderedByBorrowCount(DateTime startingFrom)
    {
        var loans = GetLoans(startingFrom);

        var bookBorrowCounts = from loan in loans
            group loan by loan.Book
            into bookGroup
            select new BookBorrowCountDto(bookGroup.Key, bookGroup.ToList().Count);

        return bookBorrowCounts.OrderByDescending(e => e.BorrowCount).ToList();
    }
}