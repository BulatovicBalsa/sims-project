using System.Collections.Generic;
using Hospital.Injectors;
using Hospital.Models.Books;
using Hospital.Models.Doctor;
using Hospital.Repositories.Books;
using Hospital.Serialization;

namespace Hospital.Services.Books;

public class LoanService
{
    private readonly LoanRepository _loanRepository = new LoanRepository(SerializerInjector.CreateInstance<ISerializer<Loan>>());

    public List<Loan> GetAll(Doctor member) => _loanRepository.GetAll(member);
    public void Add(Loan loan) => _loanRepository.Add(loan);
}