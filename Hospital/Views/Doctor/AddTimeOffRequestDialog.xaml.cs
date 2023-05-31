using System.Windows;
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
