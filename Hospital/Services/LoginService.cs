using System.Net;
using Hospital.Injectors;
using Hospital.Models;
using Hospital.Models.Doctor;
using Hospital.Repositories;
using Hospital.Repositories.Doctor;
using Hospital.Serialization;

namespace Hospital.Services;

public class LoginService
{
    private readonly DoctorRepository _memberRepository;
    private readonly LibrarianRepository _librarianRepository;

    public LoginService()
    {
        _memberRepository = new DoctorRepository(SerializerInjector.CreateInstance<ISerializer<Doctor>>());
        _librarianRepository = LibrarianRepository.Instance;
    }

    public Person? LoggedUser { get; set; }

    public bool AuthenticateUser(NetworkCredential credentials)
    {
        if (AuthenticateMember(credentials)) return true;
        if (AuthenticateLibrarian(credentials)) return true;

        return false;
    }

    private bool AuthenticateMember(NetworkCredential credentials)
    {
        LoggedUser = _memberRepository.GetByUsername(credentials.UserName);
        return LoggedUser != null && LoggedUser.Profile.Password == credentials.Password;
    }

    private bool AuthenticateLibrarian(NetworkCredential credentials)
    {
        LoggedUser = _librarianRepository.GetByUsername(credentials.UserName);
        return LoggedUser != null && LoggedUser.Profile.Password == credentials.Password;
    }

}
