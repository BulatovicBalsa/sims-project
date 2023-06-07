﻿using System.Net;
using System.Security.Principal;
using Hospital.Exceptions;
using HospitalCLI.Login;

namespace HospitalCLI;

public class App
{
    private static void Main()
    {
        var loginCli = new LoginCli();
        loginCli.LoginUser();
        Console.WriteLine(Thread.CurrentPrincipal?.Identity?.Name);
    }
}