﻿using System.Collections.Generic;
using System.Linq;
using Hospital.Serialization;

namespace Hospital.Repositories.Doctor;

using Models.Doctor;

public class DoctorRepository
{
    private const string FilePath = "../../../Data/doctors.csv";

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

    public List<Doctor> GetBySpecialization(string specialization)
    {
        return GetAll().Where(doctor => doctor.Specialization == specialization).ToList();
    }
}