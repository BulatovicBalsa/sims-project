using System.Collections.Generic;
using System.Linq;
using Hospital.Serialization;

namespace Hospital.Repositories.Doctor;

using Hospital.DTOs;
using Models.Doctor;
using System;

public class DoctorRepository
{
    private const string FilePath = "../../../Data/doctors.csv";
    private static DoctorRepository? _instance;

    public static DoctorRepository Instance => _instance ??=new DoctorRepository();

    private DoctorRepository() { }

    public List<Doctor> GetAll()
    {
        return Serializer<Doctor>.FromCSV(FilePath);
    }

    public Doctor? GetById(string id)
    {
        return GetAll().Find(doctor => doctor.Id == id);
    }

    public Doctor? GetByUsername(string username)
    {
        return GetAll().Find(doctor => doctor.Profile.Username == username);
    }

    public void Add(Doctor doctor)
    {
        var allDoctor = GetAll();

        allDoctor.Add(doctor);

        Serializer<Doctor>.ToCSV(allDoctor, FilePath);
    }

    public void Update(Doctor doctor)
    {
        var allDoctor = GetAll();

        var indexToUpdate = allDoctor.FindIndex(e => e.Id == doctor.Id);
        if (indexToUpdate == -1) throw new KeyNotFoundException();

        allDoctor[indexToUpdate] = doctor;

        Serializer<Doctor>.ToCSV(allDoctor, FilePath);
    }

    public void Delete(Doctor doctor)
    {
        var allDoctor = GetAll();

        var indexToDelete = allDoctor.FindIndex(e => e.Id == doctor.Id);
        if (indexToDelete == -1) throw new KeyNotFoundException();

        allDoctor.RemoveAt(indexToDelete);

        Serializer<Doctor>.ToCSV(allDoctor, FilePath);
    }

    public static void DeleteAll()
    {
        var emptyDoctorList = new List<Doctor>();
        Serializer<Doctor>.ToCSV(emptyDoctorList, FilePath);
    }

    public List<string> GetAllSpecializations()
    {
        var allDoctors = GetAll();
        return allDoctors.Select(doctor => doctor.Specialization).Distinct().ToList();
    }

    public List<Doctor> GetQualifiedDoctors(string specialization)
    {
        var allDoctors = GetAll();
        return allDoctors.Where(doctor => doctor.Specialization == specialization).ToList();
    }

    public List<Doctor> GetDoctorsByFilter(string firstName, string lastName, string specialization)
    {
        return GetAll()
            .Where(doctor =>
                doctor.FirstName.ToLower().Contains(firstName.ToLower()) &&
                doctor.LastName.ToLower().Contains(lastName.ToLower()) &&
                doctor.Specialization.ToLower().Contains(specialization.ToLower()))
            .ToList();
    }
    public List<PersonDTO> GetDoctorsByFilter(string id, string searchText)
    {
        var allDoctors = GetAll();
        var filteredDoctors = allDoctors.Where(doctor => IsDoctorMatchingFilter(doctor, id, searchText)).ToList();

        var doctorDTOs = filteredDoctors.Select(doctor => new PersonDTO
        {
            Id = doctor.Id,
            FirstName = doctor.FirstName,
            LastName = doctor.LastName,
            Role = Role.Doctor
        }).ToList();

        return doctorDTOs;
    }
    private bool IsDoctorMatchingFilter(Doctor doctor, string id, string searchText)
    {
        return doctor.Id != id &&
               (doctor.FirstName.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                doctor.LastName.Contains(searchText, StringComparison.OrdinalIgnoreCase));
    }
}
