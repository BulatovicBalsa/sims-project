using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using Hospital.Coordinators;
using Hospital.Models.Doctor;
using Hospital.Models.Examination;
using Hospital.Models.Patient;
using Hospital.Repositories.Doctor;
using Hospital.Repositories.Examinaton;
using Hospital.Repositories.Patient;
using Hospital.Services;
using Hospital.Views.Nurse.UrgentExaminations;

namespace Hospital.ViewModels.Nurse.UrgentExaminations;

public class UrgentExaminationsViewModel : ViewModelBase
{
    private readonly DoctorService _doctorService;
    private readonly PatientRepository _patientRepository;
    private readonly DoctorRepository _doctorRepository;
    private readonly TimeslotService _timeslotService;
    private readonly ExaminationRepository _examinationRepository;

    private Patient? _selectedPatient;
    private string? _selectedSpecialization;
    private bool _isOperation;

    public UrgentExaminationsViewModel()
    {
        _doctorService = new DoctorService();
        _patientRepository = new PatientRepository();
        _doctorRepository = new DoctorRepository();
        _timeslotService = new TimeslotService();
        _examinationRepository = new ExaminationRepository();

        AllPatients = _patientRepository.GetAll();
        AllSpecializations = _doctorService.GetAllSpecializations();

        CreateUrgentExaminationCommand = new ViewModelCommand(ExecuteCreateUrgentExaminationCommand,
            CanExecuteCreateUrgentExaminationCommand);
    }

    public List<Patient> AllPatients { get; }

    public List<string> AllSpecializations { get; }

    public Patient? SelectedPatient
    {
        get => _selectedPatient;
        set
        {
            _selectedPatient = value;
            OnPropertyChanged(nameof(SelectedPatient));
        }
    }

    public string? SelectedSpecialization
    {
        get => _selectedSpecialization;
        set
        {
            _selectedSpecialization = value;
            OnPropertyChanged(nameof(SelectedSpecialization));
        }
    }

    public bool IsOperation
    {
        get => _isOperation;
        set
        {
            _isOperation = value;
            OnPropertyChanged(nameof(IsOperation));
        }
    }

    public ICommand CreateUrgentExaminationCommand { get; }

    private void ExecuteCreateUrgentExaminationCommand(object obj)
    {
        var qualifiedDoctors = _doctorService.GetQualifiedDoctors(SelectedSpecialization);
        var earliestFreeTimeslotDoctors = _timeslotService.GetEarliestFreeTimeslotDoctors(qualifiedDoctors);
        var earliestFreeTimeslotDoctor = earliestFreeTimeslotDoctors.First();

        if (_timeslotService.IsIn2Hours(earliestFreeTimeslotDoctor.Key))
        {
            _examinationRepository.Add(new Examination(earliestFreeTimeslotDoctor.Value, SelectedPatient, IsOperation, earliestFreeTimeslotDoctor.Key, null, true), false);

            MessageBox.Show("Urgent examination successfully created", "Success");
            return;
        }

        // nema u naredna dva sata
        var postponableExaminations = new List<Examination>();
        foreach (var (timeslot, doctor) in earliestFreeTimeslotDoctors)
        {
            var doctorExaminations = _examinationRepository.GetAll(doctor);
            var nonUrgentUpcomingExaminations = doctorExaminations
                .Where(examination => examination.Start > DateTime.Now && !examination.Urgent)
                .OrderBy(examination => examination.Start).ToList();

            postponableExaminations.AddRange(nonUrgentUpcomingExaminations);
            if (postponableExaminations.Count >= 5)
                break;
        }

        if (postponableExaminations.Count > 5)
            postponableExaminations = postponableExaminations.Take(5).ToList();

        if (postponableExaminations.Count == 0)
        {
            MessageBox.Show("There are no examinations that can be postponed", "Error");
            return;
        }

        var postponeExaminationViewModel = new PostponeExaminationViewModel(postponableExaminations, earliestFreeTimeslotDoctors);
        postponeExaminationViewModel.DialogClosed += (cancelled, newTimeslot, freeDoctor) =>
        {
            if (cancelled)
                return;

            _examinationRepository.Add(new Examination(freeDoctor, SelectedPatient, IsOperation, newTimeslot ?? DateTime.MinValue, null), false);
        };
        var postponeExaminationDialog = new PostponeExaminationDialogView()
        {
            DataContext = postponeExaminationViewModel
        };
        postponeExaminationDialog.ShowDialog();
    }

    private bool CanExecuteCreateUrgentExaminationCommand(object obj)
    {
        return SelectedPatient != null && SelectedSpecialization != null;
    }

}