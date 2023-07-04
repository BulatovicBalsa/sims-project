using System.Net;
using Hospital.Injectors;
using Hospital.Models;
using Hospital.Models.Doctor;
using Hospital.Repositories;
using Hospital.Repositories.Doctor;
using Hospital.Serialization;
using Member = Hospital.Models.Doctor.Member;
using MemberRepository = Hospital.Repositories.Doctor.MemberRepository;

namespace Hospital.Services;

public class LoginService
{
    private readonly MemberRepository _memberRepository;
    private readonly LibrarianRepository _librarianRepository;

    public LoginService()
    {
        _memberRepository = new MemberRepository(SerializerInjector.CreateInstance<ISerializer<Member>>());
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
