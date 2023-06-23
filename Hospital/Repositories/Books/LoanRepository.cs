using System;
using System.Collections.Generic;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using Hospital.Exceptions;
using Hospital.Injectors;
using Hospital.Models.Books;
using Hospital.Repositories.Doctor;
using Hospital.Repositories.Manager;
using Hospital.Scheduling;
using Hospital.Serialization;

namespace Hospital.Repositories.Books;
using Hospital.Models.Doctor;
public sealed class LoanReadMapper : ClassMap<Loan>
{
    public LoanReadMapper()
    {
        Map(loan => loan.Id).Index(0);
        Map(loan => loan.Book).Index(1).TypeConverter<BookTypeConverter>();
        Map(loan => loan.Member).Index(2).TypeConverter<DoctorTypeConverter>();
        Map(loan => loan.Start).Index(3);
        Map(loan => loan.End).Index(4);
    }

    public class DoctorTypeConverter : DefaultTypeConverter
    {
        public override object? ConvertFromString(string? inputText, IReaderRow rowData, MemberMapData mappingData)
        {
            var memberId = inputText?.Trim();
            if (string.IsNullOrEmpty(memberId))
                return null;
            
            var doctor =
                new DoctorRepository(SerializerInjector.CreateInstance<ISerializer<Doctor>>()).GetById(memberId) ??
                throw new KeyNotFoundException($"Doctor with ID {memberId} not found");
            return doctor;
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

public class LoanRepository
{
    private const string FilePath = "../../../Data/loans.csv";
    private readonly ISerializer<Loan> _serializer;

    private LoanRepository(ISerializer<Loan> serializer)
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
        var allLoan = GetAll();

        if (!IsFree(loan.Book!, loan.Start)) throw new BookAlreadyLoanedException("Book is already loaned");

        allLoan.Add(loan);

       _serializer.Save(allLoan, FilePath, new LoanWriteMapper());
    }

    public void Update(Loan loan, bool isMadeByBook)
    {
        var allLoan = GetAll();

        var indexToUpdate = allLoan.FindIndex(e => e.Id == loan.Id);
        if (indexToUpdate == -1) throw new KeyNotFoundException();

        if (!IsFree(loan.Doctor, loan.Start, loan.Id))
            throw new DoctorBusyException("Doctor is busy");
        if (!IsFree(loan.Book!, loan.Start, loan.Id))
            throw new BookAlreadyLoanedException("Book is busy");
        if (isMadeByBook)
        {
            ValidateLoanTiming(loan.Start);
            ValidateMaxChangesOrDeletesLast30Days(loan.Book!);
            ValidateMaxAllowedLoansLast30Days(loan.Book!);
            BookLoanLog log = new(loan.Book!, false);
            _loanChangesTracker.Add(log);
        }

        allLoan[indexToUpdate] = loan;

       _serializer.Save(allLoan, FilePath, new LoanWriteMapper());
    }

    public void Delete(Loan loan, bool isMadeByBook)
    {
        var allLoan = GetAll();

        var indexToDelete = allLoan.FindIndex(e => e.Id == loan.Id);
        if (indexToDelete == -1) throw new KeyNotFoundException();

        if (loan.Start != DateTime.MinValue)
        {
            if (IsFree(loan.Doctor, loan.Start))
                throw new DoctorNotBusyException("Doctor is not busy,although he should be");
            if (IsFree(loan.Book, loan.Start))
                throw new BookNotBusyException("Book is not busy,although he should be");
        }

        if (isMadeByBook)
        {
            ValidateLoanTiming(loan.Start);
            ValidateMaxChangesOrDeletesLast30Days(loan.Book!);
            ValidateMaxAllowedLoansLast30Days(loan.Book!);
            BookLoanLog log = new(loan.Book!, false);
            _loanChangesTracker.Add(log);
        }

        allLoan.RemoveAt(indexToDelete);

       _serializer.Save(allLoan, FilePath, new LoanWriteMapper());
    }

    public void Delete(Doctor doctor, TimeRange timeRange)
    {
        foreach (var loan in GetLoansInTimeRange(doctor, timeRange)) Delete(loan, false);
    }

    public List<Loan> GetAll(Doctor doctor)
    {
        var doctorLoans = GetAll()
            .Where(loan => loan.Doctor?.Equals(doctor) ?? false)
            .ToList();
        return doctorLoans;
    }

    public List<Loan> GetAll(Book book)
    {
        var bookLoans = GetAll()
            .Where(loan => loan.Book!.Equals(book))
            .ToList();
        return bookLoans;
    }

    public List<Loan> GetFinishedLoans(Doctor doctor)
    {
        var currentTime = DateTime.Now;
        var finishedLoans = GetAll()
            .Where(loan => (loan.Doctor?.Equals(doctor) ?? false) && loan.Start < currentTime)
            .ToList();
        return finishedLoans;
    }

    public List<Loan> GetLoansForDate(Doctor doctor, DateTime requestedDate)
    {
        return GetAll(doctor).Where(loan => loan.Start.Date == requestedDate.Date).ToList();
    }


    public List<Loan> GetLoansInTimeRange(Doctor doctor, TimeRange range)
    {
        return GetAll(doctor)
            .Where(loan => range.DoesOverlapWith(new TimeRange(loan.Start, loan.End))).ToList();
    }

    public List<Loan> GetLoansForDate(Book book, DateTime requestedDate)
    {
        return GetAll(book).Where(loan => loan.Start.Date == requestedDate.Date).ToList();
    }

    public List<Loan> GetLoansForNextThreeDays(Doctor doctor)
    {
        var filter = new DoctorLoansFilter();

        var start = DateTime.Now;
        var end = start.AddDays(2);
        var loans = GetAll(doctor);

        return filter.Filter(loans, new LoanPerformingSpecification(start, end));
    }

    public bool IsFree(Doctor? doctor, DateTime start, string? loanId = null)
    {
        if (doctor == null)
            return true;

        var allLoans = GetAll(doctor);
        var isAvailable = !allLoans.Any(loan =>
            loan.Id != loanId && loan.DoesInterfereWith(start));

        return isAvailable;
    }

    public bool IsFree(Book book, DateTime start, string? loanId = null)
    {
        var allLoans = GetAll(book);
        var isLoaned = !allLoans.Any(loan =>
            loan.Id != loanId && loan.DoesInterfereWith(start));

        return isLoaned;
    }

    private void ValidateLoanTiming(DateTime start)
    {
        if (start < DateTime.Now.AddDays(Book.MinimumDaysToChangeOrDeleteLoan))
            throw new InvalidOperationException(
                $"It is not possible to update an loan less than {Book.MinimumDaysToChangeOrDeleteLoan * 24} hours in advance.");
    }

    private void ValidateMaxChangesOrDeletesLast30Days(Book book)
    {
        if (_loanChangesTracker.GetNumberOfChangeLogsForBookInLast30Days(book) + 1 >
            Book.MaxChangesOrDeletesLast30Days)
            throw new InvalidOperationException("Book made too many changes in last 30 days");
    }

    private void ValidateMaxAllowedLoansLast30Days(Book book)
    {
        if (_loanChangesTracker.GetNumberOfCreationLogsForBookInLast30Days(book) + 1 >
            Book.MaxAllowedLoansLast30Days)
            throw new InvalidOperationException("Book made too many loans in last 30 days");
    }

    public static void DeleteAll()
    {
        var emptyList = new List<Loan>();
       _serializer.Save(emptyList, FilePath, new LoanWriteMapper());
    }
}