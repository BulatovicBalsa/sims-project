using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Hospital.Views
{
    public partial class PatientView : Window
    {
        public PatientView()
        {
            InitializeComponent();
        }

        private void LoginView_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void BtnMinimize_OnClick(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void BtnClose_OnClick_OnClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }


    }
}
