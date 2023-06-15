using Hospital.GUI.Views;
using Hospital.Injectors;
using Hospital.PatientHealthcare.Repositories;
using Hospital.Serialization;
using Hospital.Workers.Models;
using Hospital.Workers.Repositories;
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

                var patientView = new PatientCli(patient);

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
                var doctor = new DoctorRepository(SerializerInjector.CreateInstance<ISerializer<Doctor>>()).GetById(id);
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