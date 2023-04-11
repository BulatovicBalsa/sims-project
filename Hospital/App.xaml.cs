using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Hospital.Repositories.Patient;
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
                    throw new NotImplementedException();
                }

                else if (role == "MANAGER")
                {
                    throw new NotImplementedException();
                }

                else if (role == "DOCTOR")
                {
                    throw new NotImplementedException();
                }

                loginView.Close();
            };
        }
    }
}
