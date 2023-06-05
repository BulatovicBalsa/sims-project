using GalaSoft.MvvmLight.Command;
using Hospital.Exceptions;
using Hospital.Models.Doctor;
using Hospital.Models.Examination;
using Hospital.Models.Patient;
using Hospital.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Hospital.Services;
using Hospital.DTOs;

namespace Hospital.ViewModels;

public class DoctorViewModel : ViewModelBase
{
    private readonly DoctorService _doctorService = new();
    private readonly ExaminationService _examinationService = new();
    private readonly PatientService _patientService = new();

    private const string Placeholder = "Search...";

    private Doctor _doctor;
    private ObservableCollection<Examination> _examinations;

    private ObservableCollection<Patient> _patients;

    private string _searchBoxText;

    private object _selectedPatient;

    private DateTime _selectedDate;

    public DoctorViewModel(Doctor doctor)
    {
        _doctor = doctor;
        _selectedDate = DateTime.Now;
        Patients = new ObservableCollection<Patient>(_examinationService.GetViewedPatients(doctor));
        Examinations = new ObservableCollection<Examination>(_examinationService.GetExaminationsForNextThreeDays(doctor));
        SearchBoxText = Placeholder;

        ViewMedicalRecordCommand = new RelayCommand<string>(ViewMedicalRecord);
        AddExaminationCommand = new RelayCommand(AddExamination);
        UpdateExaminationCommand = new RelayCommand(UpdateExamination);
        DeleteExaminationCommand = new RelayCommand(DeleteExamination);
        PerformExaminationCommand = new RelayCommand(PerformExamination);
        DefaultExaminationViewCommand = new RelayCommand(DefaultExaminationView);
        SendMessageCommand = new RelayCommand(SendMessage);
    }

    private void DefaultExaminationView()
    {
        Examinations.Clear();
        _examinationService.GetExaminationsForNextThreeDays(_doctor).ToList().ForEach(Examinations.Add);
    }

    public ObservableCollection<Examination> Examinations
    {
        get => _examinations;
        set
        {
            _examinations = value;
            OnPropertyChanged(nameof(Examinations));
        }
    }

    public ObservableCollection<Patient> Patients
    {
        get => _patients;
        set
        {
            _patients = value;
            OnPropertyChanged(nameof(Patients));
        }
    }

    public Doctor Doctor
    {
        get => _doctor;
        set
        {
            _doctor = value;
            OnPropertyChanged(nameof(Doctor));
        }
    }

    public string SearchBoxText
    {
        get => _searchBoxText;
        set
        {
            _searchBoxText = value;
            OnPropertyChanged(SearchBoxText);
        }
    }

    public object SelectedPatient
    {
        get => _selectedPatient;
        set
        {
            _selectedPatient = value;
            OnPropertyChanged(nameof(SelectedPatient));
        }
    }

    public DateTime SelectedDate
    {
        get => _selectedDate;
        set
        {
            _selectedDate = value;
            OnPropertyChanged(nameof(SelectedDate));
            Examinations.Clear();
            _examinationService.GetExaminationsForDate(_doctor, SelectedDate).ToList().ForEach(Examinations.Add);
        }
    }

    public object SelectedExamination { get; set; }

    public string DoctorName => $"Doctor {Doctor.FirstName} {Doctor.LastName}";

    public ICommand ViewMedicalRecordCommand { get; set; }
    public ICommand AddExaminationCommand { get; set; }
    public ICommand PerformExaminationCommand { get; set; }
    public ICommand UpdateExaminationCommand { get; set; }
    public ICommand DeleteExaminationCommand { get; set; }
    public ICommand DefaultExaminationViewCommand { get; set; }
    public ICommand SendMessageCommand { get; set; }

    private void ViewMedicalRecord(string patientId)
    {
        var patient = _patientService.GetPatientById(patientId);
        if (patient == null)
        {
            MessageBox.Show("Please select examination in order to delete it");
            return;
        }

        var dialog = new MedicalRecordDialog(patient, false);
        dialog.ShowDialog();
    }

    private void AddExamination()
    {
        var dialog = new ModifyExaminationDialog(Doctor, Examinations)
        {
            WindowStyle = WindowStyle.ToolWindow
        };

        var result = dialog.ShowDialog();
        if (result == true) MessageBox.Show("Succeed");
    }

    private void UpdateExamination()
    {
        var examinationToChange = SelectedExamination as Examination;
        if (examinationToChange == null)
        {
            MessageBox.Show("Please select examination in order to delete it");
            return;
        }

        var dialog = new ModifyExaminationDialog(Doctor, Examinations, examinationToChange)
        {
            WindowStyle = WindowStyle.ToolWindow
        };

        var result = dialog.ShowDialog();

        if (result == true) MessageBox.Show("Succeed");
    }

    private void DeleteExamination()
    {
        var examination = SelectedExamination as Examination;
        if (examination == null)
        {
            MessageBox.Show("Please select examination in order to delete it");
            return;
        }

        try
        {
            _examinationService.DeleteExamination(examination, false);
        }
        catch (DoctorNotBusyException ex)
        {
            MessageBox.Show(ex.Message);
            return;
        }
        catch (PatientNotBusyException ex)
        {
            MessageBox.Show(ex.Message);
            return;
        }

        Examinations.Remove(examination);
        MessageBox.Show("Succeed");
    }

    private void PerformExamination()
    {
        var examinationToPerform = SelectedExamination as Examination;
        if (examinationToPerform == null)
        {
            MessageBox.Show("Please select examination in order to perform it");
            return;
        }

        var patientOnExamination = _patientService.GetPatient(examinationToPerform);

        if (!examinationToPerform.IsPerformable())
        {
            MessageBox.Show("Chosen examination can't be performed right now");
            return;
        }

        if (examinationToPerform.Room is null)
        {
            MessageBox.Show("Chosen examination doesn't have room. Please add room");
            return;
        }

        var dialog = new PerformExaminationDialog(examinationToPerform, patientOnExamination);
        dialog.ShowDialog();
    }
    private void SendMessage()
    {
        PersonDTO loggedUser = new PersonDTO(_doctor.Id,_doctor.FirstName,_doctor.LastName,Role.Doctor);
        var communicationView = new CommunicationView(loggedUser);
        communicationView.ShowDialog();
    }
}