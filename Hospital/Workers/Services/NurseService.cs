﻿using System.Collections.Generic;
using System.Threading;
using Hospital.DTOs;
using Hospital.Workers.Models;
using Hospital.Workers.Repositories;

namespace Hospital.Workers.Services;

public class NurseService
{
    private readonly NurseRepository _nurseRepository;

    public NurseService()
    {
        _nurseRepository = NurseRepository.Instance;
    }

    public List<Nurse> GetAll()
    {
        return _nurseRepository.GetAll();
    }

    public List<PersonDTO> GetNursesAsPersonDTOsByFilter(string id, string searchText)
    {
        return _nurseRepository.GetNursesAsPersonDTOsByFilter(id, searchText);
    }

    public PersonDTO GetLoggedInNurse()
    {
        var identityName = Thread.CurrentPrincipal.Identity.Name;
        var id = identityName.Split("|")[0];
        var loggedInNurse = _nurseRepository.GetById(id);

        // Convert the Nurse object to PersonDTO
        return new PersonDTO(loggedInNurse.Id, loggedInNurse.FirstName, loggedInNurse.LastName, Role.Nurse);
    }
}