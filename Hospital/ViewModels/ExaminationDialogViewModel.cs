using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Mime;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Hospital.Models.Doctor;
using Hospital.Models.Examination;
using Hospital.Models.Patient;
using Hospital.Repositories.Doctor;
using Hospital.Repositories.Examinaton;
using Hospital.Repositories.Patient;

namespace Hospital.ViewModels
{
    public class ExaminationDialogViewModel : INotifyPropertyChanged
    {
        private Examination _examination;
        private PatientViewModel _patientViewModel;
        private Patient _patient;
        private IEnumerable<Hospital.Models.Doctor.Doctor> _recommendedDoctors;
        private bool _isUpdate;
        private DateTime? _selectedDate;

        public event PropertyChangedEventHandler? PropertyChanged;
        public string SelectedTime
        {
            get { return Examination.Start.ToString("HH:mm"); }
            set
            {
                TimeSpan time;
                if (TimeSpan.TryParse(value, out time))
                {
                    Examination.Start = Examination.Start.Date + time;
                    OnPropertyChanged();
                }
            }
        }
        public DateTime? SelectedDate
        {
            get { return _selectedDate; }
            set
            {
                _selectedDate = value;
                OnPropertyChanged();

                if (_selectedDate.HasValue)
                {
                    Examination.Start = _selectedDate.Value.Date.Add(Examination.Start.TimeOfDay);
                }
            }
        }
        public Patient Patient
        {
            get { return _patient;}
            set
            {
                _patient = value;
                OnPropertyChanged();
            }
        }
        public Examination Examination
        {
            get { return _examination;}
            set
            {
                _examination = value;
                OnPropertyChanged();
            }
        }

        public IEnumerable<Hospital.Models.Doctor.Doctor> RecommendedDoctors
        {
            get { return _recommendedDoctors; }
            set
            {
                _recommendedDoctors = value;
                OnPropertyChanged();
            }
        }

        public bool IsUpdate
        {
            get { return _isUpdate; }
            set
            {
                _isUpdate = value;
                OnPropertyChanged();
                Title = IsUpdate ? "Update Examination" : "Add Examination";
            }
        }

        public string Title { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand CancelCommand { get; set; }


        public ExaminationDialogViewModel(Patient patient,PatientViewModel patientViewModel)
        {
            _patientViewModel = patientViewModel;
            RecommendedDoctors = new DoctorRepository().GetAll();
            Examination = new Examination();
            SelectedDate = DateTime.Now;
            Examination.Patient = patient;
            SaveCommand = new RelayCommand(Save);
            Patient = patient;
            CancelCommand = new RelayCommand(Cancel);
        }

        public ExaminationDialogViewModel(Patient patient,Examination examination, PatientViewModel patientViewModel)
        {
            _patientViewModel = patientViewModel;
            RecommendedDoctors = new DoctorRepository().GetAll(); 
            Examination = examination.DeepCopy();
            SelectedDate = Examination.Start;
            Patient = patient;
            IsUpdate = true;
            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(Cancel);
        }

        private void Save()
        {
            if (!IsUpdate)
            {
                try
                {
                    _patientViewModel.AddExamination(Examination);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error");
                }
            }
            else
            {
                try
                {
                    _patientViewModel.UpdateExamination(Examination);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error");

                    if (ex.Message.Contains("Patient made too many changes in last 30 days") || ex.Message.Contains("Patient made too many examinations in last 30 days"))
                    {
                        Patient.IsBlocked = true;
                        new PatientRepository().Update(Patient);
                        Application.Current.Shutdown();
                    }

                    _patientViewModel.RefreshExaminations(_patient);
                    return; 
                }
                _patientViewModel.RefreshExaminations(_patient);

                
            }
            Close();
        }

        private void Cancel()
        {
            Close();
        }

        private void Close()
        {
            var window = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);
            window?.Close();
        }



        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        
    }
}
