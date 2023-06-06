using Hospital.ViewModels.Feedback;
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

namespace Hospital.Views.Feedback
{
    /// <summary>
    /// Interaction logic for HospitalFeedbackView.xaml
    /// </summary>
    public partial class HospitalFeedbackView : Window
    {
        public HospitalFeedbackView()
        {
            InitializeComponent();
            DataContext = new HospitalFeedbackViewModel();
        }
    }
}
