using System.Collections.Generic;
using System.Linq;
using Hospital.Injectors;
using Hospital.Models;
using Hospital.Models.Books;
using Hospital.Repositories.Books;
using Hospital.Serialization;

namespace Hospital.Services.Books;

public class LoanService
{
    private readonly BookRepository _bookRepository = new(SerializerInjector.CreateInstance<ISerializer<Book>>());
    private readonly LoanRepository _loanRepository = new(SerializerInjector.CreateInstance<ISerializer<Loan>>());
    private readonly CopyRepository _copyRepository = new(new JsonSerializer<Copy>());

    public List<Loan> GetAll(Member member)
    {
        return _loanRepository.GetAll(member);
    }

    public void Add(Loan loan)
    {
        _loanRepository.Add(loan);
        var copy = _copyRepository.GetByInventoryNumber(loan.InventoryNumber);
        copy.Borrow(loan.Member);
        _copyRepository.Update(copy);
    }

    public List<Book> GetNotLoanedBooks()
    {
        return _bookRepository.GetAll();
    }
    public List<Copy> GetAvailableCopies(Book book)
    {
        return _copyRepository.GetAll().Where(copy => copy.Book.Id == book.Id && copy.IsAvailable()).ToList();
    }

    public void DeleteLoan(Loan loan)
    {
        _loanRepository.Delete(loan);
    }

    public void Update(Loan loan)
    {
        _loanRepository.Update(loan);
    }

    public List<Loan> GetCurrentLoans(Member member)
    {
        return _loanRepository.GetCurrentLoans(member);
    }

    public void Return(Loan loan)
    {
        loan.Return();
        Update(loan);
        var copy = _copyRepository.GetByInventoryNumber(loan.InventoryNumber);
        copy.Return();
        _copyRepository.Update(copy);
    }
}