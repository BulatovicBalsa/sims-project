using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Hospital.Models.Doctor;
using Hospital.Repositories.Doctor;
using Hospital.Views;
using Hospital.Repositories.Patient;
using Hospital.Views;
using Hospital.Views.Manager;
using Hospital.Views.Nurse;

namespace Hospital
{
    /// <summary>
    /// Interaction logic for App.xaml
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
                    var id = identityName.Split("|")[0];
                    var patient = new PatientRepository().GetById(id);
                    if (patient == null)
                    {
                        MessageBox.Show("Login was not successful.");
                        return;
                    }
                    var patientView = new PatientView(patient);
                    patientView.Show();
                }

                else if (role == "NURSE")
                {
                    var nurseView = new NurseView();
                    nurseView.Show();
                }

                else if (role == "MANAGER")
                {
                    var managerView = new ManagerView();
                    managerView.Show();
                    
                    //throw new NotImplementedException();
                }

                else if (role == "DOCTOR")
                {
                    var doctor = new DoctorRepository().GetById(id);
                    DoctorView doctorView = new DoctorView(doctor);
                    doctorView.Show();
                }

                loginView.Close();
            };
        }
    }
}
