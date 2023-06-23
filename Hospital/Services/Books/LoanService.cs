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
    private readonly LoanRepository _loanRepository = new LoanRepository(SerializerInjector.CreateInstance<ISerializer<Loan>>());
    private readonly BookRepository _bookRepository = new(SerializerInjector.CreateInstance<ISerializer<Book>>());

    public List<Loan> GetAll(Doctor member) => _loanRepository.GetAll(member);
    public void Add(Loan loan) => _loanRepository.Add(loan);
    public List<Book> GetNotLoanedBooks() => _bookRepository.GetAll().Where(_loanRepository.IsFree).ToList();
    public void DeleteLoan(Loan loan) => _loanRepository.Delete(loan);
}