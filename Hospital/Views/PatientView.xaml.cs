using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Hospital.Models.Patient;
using Hospital.Repositories.Examinaton;
using Hospital.Repositories.Patient;
using Hospital.ViewModels;

namespace Hospital.Views
{
    public partial class PatientView : Window
    {
        private PatientViewModel _viewModel;
        private ExaminationRepository _examinationRepository;

        public PatientView(Patient patient)
        {
            InitializeComponent();

            _examinationRepository =
                new ExaminationRepository(new ExaminationChangesTracker(new ExaminationChangesTrackerRepository()));
            _viewModel = new PatientViewModel(_examinationRepository);
            _viewModel.LoadExaminations(patient); 
            
            this.DataContext = _viewModel;

        }

        private void PatientView_OnMouseDown(object sender, MouseButtonEventArgs e)
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

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void BtnAddExamination_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnUpdateExamination_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnDeleteExamination_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
