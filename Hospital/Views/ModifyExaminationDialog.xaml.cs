using Hospital.Coordinators;
using Hospital.Exceptions;
using Hospital.Models.Doctor;
using Hospital.Models.Examination;
using Hospital.Models.Patient;
using Hospital.Repositories.Examinaton;
using Hospital.Repositories.Patient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
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
    public partial class ExaminationDialog : Window
    {
        private Doctor _doctor;
        private ObservableCollection<Examination> _examinationCollection;
        private bool _isUpdate = false;
        private Examination? _examinationToChange = null;

        private readonly DoctorCoordinator _coordinator = new DoctorCoordinator();

        public ExaminationDialog(Doctor doctor, ObservableCollection<Examination> examinationCollection)
        {
            InitializeComponent();

            _doctor = doctor;
            PatientComboBox.ItemsSource = GetPatients();
            _examinationCollection = examinationCollection;
        }

        public ExaminationDialog(Doctor doctor, ObservableCollection<Examination> examinationCollection, Examination examinationToChange)
        {
            InitializeComponent();

            _isUpdate = true;
            _doctor = doctor;
            PatientComboBox.ItemsSource = GetPatients();
            _examinationCollection = examinationCollection;
            _examinationToChange = examinationToChange;
            
            fillForm(examinationToChange);
        }

        private void fillForm(Examination examinationToChange)
        {
            ExaminationDatePicker.SelectedDate = examinationToChange.Start;
            IsOperation.IsChecked = examinationToChange.IsOperation;
            PatientComboBox.SelectedItem = examinationToChange.Patient;
            ConfirmButton.Content = "Update";
        }

        private List<Patient> GetPatients()
        {
            return _coordinator.GetAllPatients();
        }

        private void AddExamination_Click(object sender, RoutedEventArgs e)
        {

            var createdExamination = createExaminationFromForm();
            if (createdExamination is null) return;
            
            try
            {
                if (_isUpdate)
                {
                    updateExamination(createdExamination);
                }
                else
                {
                    _coordinator.AddExamination(createdExamination);
                    _examinationCollection.Add(createdExamination);
                }
            }
            catch (Exception ex)
            {
                if(ex is DoctorBusyException || ex is PatientBusyException)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
            }

            DialogResult = true;
        }

        private Examination? createExaminationFromForm()
        {
            Patient? patient = PatientComboBox.SelectedItem as Patient;
            if (patient == null)
            {
                MessageBox.Show("You must select patient");
                return null;
            }

            DateTime? startDate = ExaminationDatePicker.SelectedDate;
            if (startDate == null)
            {
                MessageBox.Show("You must select date and time");
                return null;
            }

            bool? isOperationNullable = IsOperation.IsChecked;


            return new Examination(_doctor, patient, isOperationNullable.GetValueOrDefault(), startDate.GetValueOrDefault());
        }

        private void updateExamination(Examination examination)
        {
            examination.Id = _examinationToChange.Id;
            _coordinator.UpdateExamination(examination);
            _examinationCollection.Clear();
            foreach (var examinationToAdd in _coordinator.GetExaminationsForNextThreeDays(_doctor))
            {
                _examinationCollection.Add(examinationToAdd);
            }
        }
    }
}
