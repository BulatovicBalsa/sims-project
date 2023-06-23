using System.Collections.Generic;
using System.Linq;
using Hospital.Injectors;
using Hospital.Models.Books;
using Hospital.Models.Doctor;
using Hospital.Repositories.Books;
using Hospital.Serialization;

namespace Hospital.Services.Books;

public class LoanService
{
    private readonly BookRepository _bookRepository = new(SerializerInjector.CreateInstance<ISerializer<Book>>());
    private readonly LoanRepository _loanRepository = new(SerializerInjector.CreateInstance<ISerializer<Loan>>());

    public List<Loan> GetAll(Doctor member)
    {
        return _loanRepository.GetAll(member);
    }

    public void Add(Loan loan)
    {
        _loanRepository.Add(loan);
    }

    public List<Book> GetNotLoanedBooks()
    {
        return _bookRepository.GetAll().Where(_loanRepository.IsFree).ToList();
    }

    public void DeleteLoan(Loan loan)
    {
        _loanRepository.Delete(loan);
    }

    public void Update(Loan loan)
    {
        _loanRepository.Update(loan);
    }

    public List<Loan> GetCurrentLoans(Doctor doctor)
    {
        return _loanRepository.GetCurrentLoans(doctor);
    }
}