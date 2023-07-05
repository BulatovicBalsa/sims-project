using System.Net;
using Library.Models;
using Library.Repositories;

namespace Library.Services;

public class LoginService
{
    private readonly MemberRepository _memberRepository;
    private readonly LibrarianRepository _librarianRepository;

    public LoginService()
    {
        _memberRepository = new MemberRepository();
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
