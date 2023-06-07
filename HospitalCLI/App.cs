using System.Diagnostics;
using System.Net;
using System.Security.Principal;
using Hospital.Exceptions;
using HospitalCLI.Login;

namespace HospitalCLI;

public class App
{
    private static void Main()
    {
        var loginCli = new LoginCli();
        var isLoginSucceed = loginCli.LoginUser();
        if (!isLoginSucceed)
        {
            Console.WriteLine("Login failed");
            return;
        }

        Console.WriteLine(Thread.CurrentPrincipal?.Identity?.Name);
    }
}