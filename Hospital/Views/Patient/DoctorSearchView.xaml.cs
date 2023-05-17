using Hospital.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Hospital.Views
{
    public partial class DoctorSearchView : Window
    {
        private DoctorSearchViewModel _viewModel;

        public DoctorSearchView()
        {
            InitializeComponent();

            _viewModel = new DoctorSearchViewModel();
            this.DataContext = _viewModel;
        }
    }
}
