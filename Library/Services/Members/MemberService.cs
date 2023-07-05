using System.Collections.Generic;
using Library.Models;
using Library.Repositories;

namespace Library.Services.Members;

public class MemberService
{
    private readonly MemberRepository _memberRepository;

    public MemberService()
    {
        _memberRepository = new MemberRepository();
    }

    public List<Member> GetAll()
    {
        return _memberRepository.GetAll(); 
    }
}