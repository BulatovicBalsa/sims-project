using System;
using System.Globalization;
using System.Threading;
using System.Timers;
using System.Windows;
using Hospital.Injectors;
using Hospital.Models.Doctor;
using Hospital.Repositories.Doctor;
using Hospital.Repositories.Patient;
using Hospital.Serialization;
using Hospital.Services;
using Hospital.Services.Manager;
using Hospital.Views;
using Hospital.Views.Manager;
using Hospital.Views.Librarian;

namespace Hospital;

public partial class App : Application
{
    private const string _unsuccessfulLoginMessage = "Login was not successful.";
    
    protected void ApplicationStart(object sender, EventArgs e)
    {
        CultureInfo.CurrentCulture = new CultureInfo("sr-RS");

        var loginView = new LoginView();
        loginView.Show();
        loginView.IsVisibleChanged += (s, ev) =>
        {
            if (loginView.IsVisible || !loginView.IsLoaded) return;

            var identityName = Thread.CurrentPrincipal.Identity.Name;
            var id = identityName.Split("|")[0];
            var role = identityName.Split("|")[1];

            if (role == "PATIENT")
            {
                var patient = PatientRepository.Instance.GetById(id);
                if (patient == null)
                {
                    MessageBox.Show(_unsuccessfulLoginMessage);
                    return;
                }

                if (patient.IsBlocked)
                {
                    MessageBox.Show("Your profile is blocked.");
                    return;
                }

                var patientView = new PatientView(patient);
                patientView.Show();
            }

            else if (role == "LIBRARIAN")
            {
                var librarianView = new LibrarianMainView();
                librarianView.Show();
            }

            else if (role == "MANAGER")
            {
                var managerView = new ManagerView();
                managerView.Show();
            }

            else if (role == "DOCTOR")
            {
                var doctor = new DoctorRepository(SerializerInjector.CreateInstance<ISerializer<Doctor>>()).GetById(id);
                if (doctor == null)
                {
                    MessageBox.Show(_unsuccessfulLoginMessage);
                    return;
                }

                var doctorView = new DoctorView(doctor);
                doctorView.Show();
            }

            loginView.Close();
        };
    }
}