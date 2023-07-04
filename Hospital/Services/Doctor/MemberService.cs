using System.Collections.Generic;
using Hospital.Models;
using Hospital.Repositories;

namespace Hospital.Services;

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