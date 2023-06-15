﻿using System.Net;
using Hospital.Core.Accounts.Models;
using Hospital.Core.PatientHealthcare.Repositories;
using Hospital.Core.Workers.Models;
using Hospital.Core.Workers.Repositories;
using Hospital.Injectors;
using Hospital.Serialization;

namespace Hospital.Core.Accounts.Services;

public class LoginService
{
    private readonly DoctorRepository _doctorRepository;
    private readonly NurseRepository _nurseRepository;
    private readonly PatientRepository _patientRepository;

    public LoginService()
    {
        _doctorRepository =
            new DoctorRepository(SerializerInjector.CreateInstance<ISerializer<Doctor>>());
        _nurseRepository = NurseRepository.Instance;
        _patientRepository = PatientRepository.Instance;
    }

    public Person? LoggedUser { get; set; }

    public bool AuthenticateUser(NetworkCredential credentials)
    {
        if (AuthenticateDoctor(credentials)) return true;
        if (AuthenticateNurse(credentials)) return true;
        if (AuthenticatePatient(credentials)) return true;
        if (AuthenticateManager(credentials)) return true;

        return false;
    }

    private bool AuthenticateDoctor(NetworkCredential credentials)
    {
        LoggedUser = _doctorRepository.GetByUsername(credentials.UserName);
        return LoggedUser != null && LoggedUser.Profile.Password == credentials.Password;
    }

    private bool AuthenticateNurse(NetworkCredential credentials)
    {
        LoggedUser = _nurseRepository.GetByUsername(credentials.UserName);
        return LoggedUser != null && LoggedUser.Profile.Password == credentials.Password;
    }

    private bool AuthenticatePatient(NetworkCredential credentials)
    {
        LoggedUser = _patientRepository.GetByUsername(credentials.UserName);
        return LoggedUser != null && LoggedUser.Profile.Password == credentials.Password;
    }

    private bool AuthenticateManager(NetworkCredential credentials)
    {
        if (credentials.UserName != "manager" || credentials.Password != "manager") return false;

        LoggedUser = new Manager();
        return true;
    }
}