using System.Diagnostics;
using System.Net;
using System.Security.Principal;
using Hospital.Exceptions;
using Hospital.Repositories.Doctor;
using Hospital.Repositories.Patient;
using Hospital.Views.Manager;
using Hospital.Views.Nurse;
using Hospital.Views;
using HospitalCLI.CliViews;
using HospitalCLI.Login;

namespace HospitalCLI;

public class App
{
    private const string UnsuccessfulLoginMessage = "Login was not successful.";

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
        OpenUserCli();
    }

    private static void OpenUserCli()
    {
        var identityName = Thread.CurrentPrincipal?.Identity?.Name;
        var id = identityName!.Split("|")[0];
        var role = identityName!.Split("|")[1];

        switch (role)
        {
            case "PATIENT":
            {
                var patient = PatientRepository.Instance.GetById(id);
                if (patient == null)
                {
                    Console.WriteLine(UnsuccessfulLoginMessage);
                    return;
                }

                if (patient.IsBlocked)
                {
                    Console.WriteLine("Your profile is blocked.");
                    return;
                }

                var patientView = new PatientView(patient);
                patientView.Show();

                //ShowNotifications(id);
                break;
            }
            case "NURSE":
            {
                var nurseView = new NurseMainView();
                nurseView.Show();
                break;
            }
            case "MANAGER":
            {
                var managerView = new ManagerView();
                managerView.Show();
                break;
            }
            case "DOCTOR":
            {
                var doctor = DoctorRepository.Instance.GetById(id);
                if (doctor == null)
                {
                    Console.WriteLine(UnsuccessfulLoginMessage);
                    return;
                }

                var doctorView = new DoctorCli();

                //ShowNotifications(id);
                break;
            }
        }
    }
}