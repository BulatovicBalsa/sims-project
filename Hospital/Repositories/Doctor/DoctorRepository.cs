using System.Collections.Generic;
using Hospital.Serialization;

namespace Hospital.Repositories.Doctor;

using Models.Doctor;

public class DoctorRepository
{
    private const string FilePath = "../../../Data/doctors.csv";

    public List<Models.Doctor.Doctor> GetAll()
    {
        return Serializer<Models.Doctor.Doctor>.FromCSV(FilePath);
    }

    public Models.Doctor.Doctor? GetById(string id)
    {
        return GetAll().Find(doctor => doctor.Id == id);
    }

    public Models.Doctor.Doctor? GetByUsername(string username)
    {
        return GetAll().Find(doctor => doctor.Profile.Username == username);
    }

    public void Add(Models.Doctor.Doctor doctor)
    {
        var allDoctor = GetAll();

        allDoctor.Add(doctor);

        Serializer<Models.Doctor.Doctor>.ToCSV(allDoctor, FilePath);
    }

    public void Update(Models.Doctor.Doctor doctor)
    {
        var allDoctor = GetAll();

        var indexToUpdate = allDoctor.FindIndex(e => e.Id == doctor.Id);
        if (indexToUpdate == -1) throw new KeyNotFoundException();

        allDoctor[indexToUpdate] = doctor;

        Serializer<Models.Doctor.Doctor>.ToCSV(allDoctor, FilePath);
    }

    public void Delete(Models.Doctor.Doctor doctor)
    {
        var allDoctor = GetAll();

        var indexToDelete = allDoctor.FindIndex(e => e.Id == doctor.Id);
        if (indexToDelete == -1) throw new KeyNotFoundException();

        allDoctor.RemoveAt(indexToDelete);

        Serializer<Models.Doctor.Doctor>.ToCSV(allDoctor, FilePath);
    }

    public static void DeleteAll()
    {
        var emptyDoctorList = new List<Models.Doctor.Doctor>();
        Serializer<Models.Doctor.Doctor>.ToCSV(emptyDoctorList, FilePath);
    }
}