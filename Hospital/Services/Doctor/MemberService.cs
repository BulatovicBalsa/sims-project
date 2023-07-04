using Hospital.Models.Doctor;
using System.Collections.Generic;
using System.Linq;
using Hospital.Repositories.Doctor;
using Hospital.DTOs;
using Hospital.Injectors;
using Hospital.Serialization;

namespace Hospital.Services;

public class MemberService
{
    private readonly MemberRepository _memberRepository;

    public MemberService()
    {
        _memberRepository = new MemberRepository(SerializerInjector.CreateInstance<ISerializer<Member>>());
    }

    public List<Member> GetAll()
    {
        return _memberRepository.GetAll(); 
    }
}