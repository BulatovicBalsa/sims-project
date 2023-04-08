using Hospital.Views.LoginView;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Hospital
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Instantiate the Login window as a modal dialog box
            Login loginWindow = new Login();
            bool? result = loginWindow.ShowDialog();

            if (result == true)
            {
                // If the user logs in successfully, set the MainWindow as the application's main window and show it
                this.Visibility = Visibility.Visible;
                Application.Current.MainWindow = this;
            }
            else
            {
                // If the user cancels the login, close the application
                Application.Current.Shutdown();
            }
        }
    }
}
