﻿using System;
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

namespace Hospital.ViewModels
{
    public class ExaminationDialogViewModel : INotifyPropertyChanged
    {
        private Examination _examination;
        private PatientViewModel _patientViewModel;
        private Patient _patient;
        private IEnumerable<Doctor> _recommendedDoctors;
        private bool _isUpdate;

        public event PropertyChangedEventHandler? PropertyChanged;

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

        public IEnumerable<Doctor> RecommendedDoctors
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
            SaveCommand = new RelayCommand(Save);
            Patient = patient;
            CancelCommand = new RelayCommand(Cancel);
        }

        public ExaminationDialogViewModel(Patient patient,Examination examination, PatientViewModel patientViewModel)
        {
            _patientViewModel = patientViewModel;
            RecommendedDoctors = new DoctorRepository().GetAll(); 
            Examination = examination;
            Patient = patient;
            IsUpdate = true;
            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(Cancel);
        }

        private void Save()
        {
            if (!IsUpdate)
            {
                _patientViewModel.AddExamination(Examination);
            }
            else
            {
                _patientViewModel.UpdateExamination(Examination);
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
