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
using Hospital.Models.Doctor;

namespace Hospital.Views
{
    public partial class AddTimeOffRequestDialog : Window
    {
        public AddTimeOffRequestDialog(Doctor doctor)
        {
            DataContext = new AddTimeOffRequestViewModel(doctor);
            InitializeComponent();
        }
    }
}
