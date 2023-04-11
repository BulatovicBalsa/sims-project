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
                    throw new NotImplementedException();
                }

                else if (role == "NURSE")
                {
                    throw new NotImplementedException();
                }

                else if (role == "MANAGER")
                {
                    throw new NotImplementedException();
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
