using Hospital.Injectors;
using Hospital.Models.Books;
using Hospital.Repositories.Books;
using Hospital.Serialization;

namespace Hospital.Services.Books;

public class LoanService
{
    private LoanRepository _bookRepository = new LoanRepository(SerializerInjector.CreateInstance<ISerializer<Loan>>());
}