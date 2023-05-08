using GalaSoft.MvvmLight.Command;
using Hospital.Coordinators;
using Hospital.Exceptions;
using Hospital.Models.Doctor;
using Hospital.Models.Examination;
using Hospital.Models.Patient;
using Hospital.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Hospital.ViewModels;

public class DoctorViewModel : ViewModelBase
{
    private readonly DoctorService _doctorService = new();
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
        Patients = new ObservableCollection<Patient>(_doctorService.GetViewedPatients(doctor));
        Examinations = new ObservableCollection<Examination>(_doctorService.GetExaminationsForNextThreeDays(doctor));
        SearchBoxText = Placeholder;

        ViewMedicalRecordCommand = new RelayCommand<string>(ViewMedicalRecord);
        AddExaminationCommand = new RelayCommand(AddExamination);
        UpdateExaminationCommand = new RelayCommand(UpdateExamination);
        DeleteExaminationCommand = new RelayCommand(DeleteExamination);
        PerformExaminationCommand = new RelayCommand(PerformExamination);
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
            _doctorService.GetExaminationsForDate(SelectedDate);
        }
    }

    public object SelectedExamination { get; set; }

    public string DoctorName => $"Doctor {Doctor.FirstName} {Doctor.LastName}";

    public ICommand ViewMedicalRecordCommand { get; set; }
    public ICommand AddExaminationCommand { get; set; }
    public ICommand PerformExaminationCommand { get; set; }
    public ICommand UpdateExaminationCommand { get; set; }
    public ICommand DeleteExaminationCommand { get; set; }

    private void ViewMedicalRecord(string patientId)
    {
        var patient = _doctorService.GetPatientById(patientId);
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
            _doctorService.DeleteExamination(examination);
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

        var patientOnExamination = _doctorService.GetPatient(examinationToPerform);

        /*if (!examination.IsPerfomable())
        {
            MessageBox.Show("Chosen examination can't be performed right now");
            return;
        }*/

        var dialog = new PerformExaminationDialog(examinationToPerform, patientOnExamination);
        dialog.ShowDialog();
    }
}