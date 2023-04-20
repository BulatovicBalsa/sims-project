using Hospital.Exceptions;
using Hospital.Models.Doctor;
using Hospital.Models.Examination;
using Hospital.Models.Patient;
using Hospital.Repositories.Examinaton;
using Hospital.Repositories.Patient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace Hospital.Views
{
    /// <summary>
    /// Interaction logic for ExaminationDialog.xaml
    /// </summary>
    public partial class ExaminationDialog : Window
    {
        private Doctor _doctor;
        private ObservableCollection<Examination> _examinations;
        private bool _isUpdate = false;
        private Examination? examinationToChange = null;

        public ExaminationDialog(Doctor doctor, ObservableCollection<Examination> examinations)
        {
            InitializeComponent();

            this._doctor = doctor;
            this.PatientComboBox.ItemsSource = GetPatients();
            this._examinations = examinations;
        }

        public ExaminationDialog(Doctor doctor, ObservableCollection<Examination> examinations, Examination examinationToChange)
        {
            InitializeComponent();

            this._isUpdate = true;
            this._doctor = doctor;
            this.PatientComboBox.ItemsSource = GetPatients();
            this._examinations = examinations;
            this.examinationToChange = examinationToChange;

            ExaminationDatePicker.SelectedDate = examinationToChange.Start;
            IsOperation.IsChecked = examinationToChange.IsOperation;
            PatientComboBox.SelectedItem = examinationToChange.Patient;
            ConfirmButton.Content = "Update";
        }

        private List<Patient> GetPatients()
        {
            return new PatientRepository().GetAll();
        }

        private void AddExamination_Click(object sender, RoutedEventArgs e)
        {
            Patient? patient = PatientComboBox.SelectedItem as Patient;
            if (patient == null)
            {
                MessageBox.Show("You must select patient");
                return;
            }

            DateTime? startDate = ExaminationDatePicker.SelectedDate;
            if (startDate == null)
            {
                MessageBox.Show("You must select date and time");
                return;
            }

            bool? isOperationNullable = IsOperation.IsChecked;


            Examination examination = new Examination(_doctor, patient, isOperationNullable.GetValueOrDefault(), startDate.GetValueOrDefault());

            
            try
            {
                if (this._isUpdate)
                {
                    new ExaminationRepository(new ExaminationChangesTracker()).Update(examination, false);
                    _examinations.Clear();
                    foreach (var examinationToAdd in new ExaminationRepository(new ExaminationChangesTracker()).GetAll())
                    {
                        _examinations.Add(examinationToAdd);
                    }
                }
                else
                {
                    new ExaminationRepository(new ExaminationChangesTracker()).Add(examination, false);
                    _examinations.Add(examination);
                }
            }
            catch (DoctorBusyException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            catch (PatientBusyException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            DialogResult = true;
        }
    }
}
