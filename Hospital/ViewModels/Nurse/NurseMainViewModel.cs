﻿using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Input;
using Hospital.Models.Patient;
using Hospital.Repositories.Patient;
using Hospital.ViewModels.Nurse.Patients;

namespace Hospital.ViewModels.Nurse;

public class NurseMainViewModel : ViewModelBase
{
    private ViewModelBase _currentChildView;
    private string _errorMessage;

    public NurseMainViewModel()
    {
        //_patients.CollectionChanged += (sender, e) =>
        //{
        //    if (e.Action != NotifyCollectionChangedAction.Add) return;
        //    if (e.NewItems == null) return;
        //    foreach (var item in e.NewItems) _patientRepository.Add((Patient)item);
        //};

        //DeletePatientCommand = new ViewModelCommand(ExecuteDeletePatientCommand, CanExecuteDeletePatientCommand);
        //UpdatePatientCommand = new ViewModelCommand(ExecuteUpdatePatientCommand, CanExecuteUpdatePatientCommand);
        ShowPatientsView = new ViewModelCommand(ExecuteShowPatientsViewCommand);

        ExecuteShowPatientsViewCommand(null);
    }

    public string ErrorMessage
    {
        get => _errorMessage;
        set
        {
            _errorMessage = value;
            OnPropertyChanged(nameof(ErrorMessage));
        }
    }

    public ViewModelBase CurrentChildView
    {
        get => _currentChildView;
        set
        {
            _currentChildView = value;
            OnPropertyChanged(nameof(CurrentChildView));
        }
    }

    //public ICommand DeletePatientCommand { get; }
    //public ICommand UpdatePatientCommand { get; }
    public ICommand ShowPatientsView { get; }

    //private bool CanExecuteDeletePatientCommand(object obj)
    //{
    //    return SelectedPatient != null;
    //}

    //private void ExecuteDeletePatientCommand(object obj)
    //{
    //    _patientRepository.Delete(SelectedPatient);
    //    _patients.Remove(SelectedPatient);
    //}

    //private bool CanExecuteUpdatePatientCommand(object obj)
    //{
    //    return SelectedPatient != null;
    //}

    //private void ExecuteUpdatePatientCommand(object obj)
    //{
    //    if (string.IsNullOrEmpty(SelectedPatient?.FirstName))
    //    {
    //        ErrorMessage = "FirstName field can't be empty";
    //    }

    //    else if (string.IsNullOrEmpty(SelectedPatient.LastName))
    //    {
    //        ErrorMessage = "LastName field can't be empty";
    //    }

    //    else if (SelectedPatient.Jmbg.Length != 13)
    //    {
    //        ErrorMessage = "JMBG field needs to have 13 characters";
    //    }

    //    else if (string.IsNullOrEmpty(SelectedPatient.Profile.Username) ||
    //             SelectedPatient.Profile.Username.Length < 4)
    //    {
    //        ErrorMessage = "Username field needs to have at least 4 characters";
    //    }

    //    else if (string.IsNullOrEmpty(SelectedPatient.Profile.Password) ||
    //             SelectedPatient.Profile.Password.Length < 4)
    //    {
    //        ErrorMessage = "Password field needs to have at least 4 characters";
    //    }

    //    else if (SelectedPatient.MedicalRecord.Height <= 0)
    //    {
    //        ErrorMessage = "Invalid height";
    //    }

    //    else if (SelectedPatient.MedicalRecord.Weight <= 0)
    //    {
    //        ErrorMessage = "Invalid weight";
    //    }
    //    else
    //    {
    //        ErrorMessage = "";
    //        _patientRepository.Update(SelectedPatient);
    //    }
    //}

    private void ExecuteShowPatientsViewCommand(object obj)
    {
        CurrentChildView = new PatientGridViewModel();
    }
}
