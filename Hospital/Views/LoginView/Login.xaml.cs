using Hospital.Models.Manager;
using Hospital.Repositories.Doctor;
using Hospital.Repositories.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Hospital.Views.LoginView
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public string UserId { get; private set; }
        public Login()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            UserId = string.Empty;
            new EquipmentRepository().Add(new Equipment("mik", Equipment.EquipmentType.FURNITURE));
            new DoctorRepository().Add(new Models.Doctor.Doctor("mik", "", "", "miki", "milan"));
        }

        private void OnLoginButtonClick(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;
            foreach (var doctor in new DoctorRepository().GetAll())
            {
                if (doctor.Profile.Username == username)
                {
                    if (doctor.Profile.Password == password)
                    {
                        UserId = doctor.Id;
                        this.DialogResult = true;
                        return;
                    }
                }
            }
        }
    }
}
