using System;
using System.Threading;
using System.Windows;
using Hospital.Repositories.Doctor;
using Hospital.Repositories.Patient;
using Hospital.Views;
using Hospital.Views.Manager;
using Hospital.Views.Nurse;

namespace Hospital;

/// <summary>
///     Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    protected void ApplicationStart(object sender, EventArgs e)
    {
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
                var patient = new PatientRepository().GetById(id);
                if (patient == null)
                {
                    MessageBox.Show("Login was not successful.");
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

            else if (role == "NURSE")
            {
                var nurseView = new NurseMainView();
                nurseView.Show();
            }

            else if (role == "MANAGER")
            {
                var managerView = new ManagerView();
                managerView.Show();
            }

            else if (role == "DOCTOR")
            {
                var doctor = new DoctorRepository().GetById(id);
                var doctorView = new DoctorView(doctor);
                doctorView.Show();
            }

            loginView.Close();
        };
    }
}