﻿using System.Collections.Generic;
using System.Linq;
using Hospital.Exceptions;
using Hospital.Serialization;

namespace Hospital.Repositories.Nurse;

using Hospital.DTOs;
using Hospital.Filter;
using Hospital.Models.Nurse;
using System;

public class NurseRepository
{
    private const string FilePath = "../../../Data/nurses.csv";
    private static NurseRepository? _instance;
    public static NurseRepository Instance => _instance ??= new NurseRepository();
    private NurseRepository() { }
    public List<Nurse> GetAll()
    {
        return Serializer<Nurse>.FromCSV(FilePath);
    }

    public Nurse? GetById(string id)
    {
        return GetAll().FirstOrDefault(nurse => nurse.Id == id);
    }

    public Nurse? GetByUsername(string username)
    {
        return GetAll().FirstOrDefault(nurse => nurse.Profile.Username == username);
    }

    public void Add(Nurse nurse)
    {
        var allNurses = GetAll();
        allNurses.Add(nurse);
        Serializer<Nurse>.ToCSV(allNurses, FilePath);
    }
    public void Update(Nurse nurse)

    {
        var allNurses = GetAll();

        var indexToUpdate = allNurses.FindIndex(nurseRecord => nurseRecord.Id == nurse.Id);
        if (indexToUpdate == -1)
            throw new ObjectNotFoundException($"Nurse with id {nurse.Id} was not found.");
        allNurses[indexToUpdate] = nurse;

        Serializer<Nurse>.ToCSV(allNurses, FilePath);
    }

    public void Delete(Nurse nurse)
    {
        var allNurses = GetAll();

        if (!allNurses.Remove(nurse))
            throw new ObjectNotFoundException($"Nurse with id {nurse.Id} was not found.");

        Serializer<Nurse>.ToCSV(allNurses, FilePath);
    }

    public List<PersonDTO> GetNursesAsPersonDTOsByFilter(string id, string searchText)
    {
        var allNurses = GetAll();
        var filteredNurses = allNurses.Where(nurse => SearchFilter.IsPersonMatchingFilter(nurse, id, searchText)).ToList();

        var nurseDTOs = filteredNurses.Select(nurse => new PersonDTO
        {
            Id = nurse.Id,
            FirstName = nurse.FirstName,
            LastName = nurse.LastName,
            Role = Role.Nurse
        }).ToList();

        return nurseDTOs;
    }
}