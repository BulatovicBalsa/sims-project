using System.Collections.Generic;
using System.Linq;
using Hospital.Injectors;
using Hospital.Serialization;

namespace Hospital.Repositories.Doctor;

using DTOs;
using Filter;
using Models.Doctor;

public class MemberRepository
{
    private const string FilePath = "../../../Data/doctors.csv";
    private readonly ISerializer<Member> _serializer;

    public MemberRepository(ISerializer<Member> serializer)
    {
        _serializer = serializer;
    }

    public List<Member> GetAll()
    {
        return _serializer.Load(FilePath);
    }

    public Member? GetById(string id)
    {
        return GetAll().Find(doctor => doctor.Id == id);
    }

    public Member? GetByUsername(string username)
    {
        return GetAll().Find(doctor => doctor.Profile.Username == username);
    }

    public void Add(Member member)
    {
        var allDoctor = GetAll();

        allDoctor.Add(member);

        _serializer.Save(allDoctor, FilePath);
    }

    public void Update(Member member)
    {
        var allDoctor = GetAll();

        var indexToUpdate = allDoctor.FindIndex(e => e.Id == member.Id);
        if (indexToUpdate == -1) throw new KeyNotFoundException();

        allDoctor[indexToUpdate] = member;

        _serializer.Save(allDoctor, FilePath);
    }

    public void Delete(Member member)
    {
        var allDoctor = GetAll();

        var indexToDelete = allDoctor.FindIndex(e => e.Id == member.Id);
        if (indexToDelete == -1) throw new KeyNotFoundException();

        allDoctor.RemoveAt(indexToDelete);

        _serializer.Save(allDoctor, FilePath);
    }

    public void DeleteAll()
    {
        var emptyDoctorList = new List<Member>();
        _serializer.Save(emptyDoctorList, FilePath);
    }

    public List<string> GetAllSpecializations()
    {
        var allDoctors = GetAll();
        return allDoctors.Select(doctor => doctor.Specialization).Distinct().ToList();
    }

    public List<Member> GetQualifiedDoctors(string specialization)
    {
        var allDoctors = GetAll();
        return allDoctors.Where(doctor => doctor.Specialization == specialization).ToList();
    }

    public List<Member> GetDoctorsByFilter(string firstName, string lastName, string specialization)
    {
        return GetAll()
            .Where(doctor =>
                doctor.FirstName.ToLower().Contains(firstName.ToLower()) &&
                doctor.LastName.ToLower().Contains(lastName.ToLower()) &&
                doctor.Specialization.ToLower().Contains(specialization.ToLower()))
            .ToList();
    }

    public List<PersonDTO> GetDoctorsAsPersonDTOsByFilter(string id, string searchText)
    {
        var allDoctors = GetAll();
        var filteredDoctors = allDoctors.Where(doctor => SearchFilter.IsPersonMatchingFilter(doctor, id, searchText)).ToList();

        var doctorDTOs = filteredDoctors.Select(doctor => new PersonDTO
        {
            Id = doctor.Id,
            FirstName = doctor.FirstName,
            LastName = doctor.LastName,
            Role = Role.Doctor
        }).ToList();

        return doctorDTOs;
    }
}
