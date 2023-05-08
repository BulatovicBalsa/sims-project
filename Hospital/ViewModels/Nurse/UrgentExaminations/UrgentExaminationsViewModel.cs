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
        var suitableDoctors = _doctorRepository.GetBySpecialization(SelectedSpecialization);

        var earliestTimeslots = suitableDoctors
            .Select(doctor => new KeyValuePair<Doctor, DateTime?>(doctor, _timeslotService.GetEarliestFreeTimeslotIn2Hours(doctor))).ToList();

        var earliestOfAll = earliestTimeslots.Min(doctorTimeslotPair => doctorTimeslotPair.Value ?? DateTime.MaxValue);

        if (earliestOfAll != DateTime.MaxValue)
        {
            var earliestTimeslotDoctor =
                earliestTimeslots.Where(doctorTimeslotPair => doctorTimeslotPair.Value == earliestOfAll).ToList()[0];
            _examinationRepository.Add(new Examination(earliestTimeslotDoctor.Key, SelectedPatient, IsOperation, earliestOfAll, null), false);

            MessageBox.Show("Urgent examination successfully created", "Success");

            return;
        }

        // nema u naredna dva sata
        var doctorFirstFreeTimeslot = suitableDoctors
            .Select(doctor => new KeyValuePair<Doctor, DateTime>(doctor, _timeslotService.GetEarliestFreeTimeslot(doctor))).ToList();
        var firstFreeTimeslot = doctorFirstFreeTimeslot.Min(doctorTimeslotPair => doctorTimeslotPair.Value);
        var firstFreeDoctorTimeslotPair =
            doctorFirstFreeTimeslot.Where(doctorTimeslotPair => doctorTimeslotPair.Value == firstFreeTimeslot);

        doctorFirstFreeTimeslot = doctorFirstFreeTimeslot.OrderBy(pair => pair.Value).ToList();
        var postponeableExaminations = new List<Examination>();

        while (postponeableExaminations.Count < 5)
        {
            foreach (var doctorTimeslotPair in doctorFirstFreeTimeslot)
            {
                var doctorExaminations = _examinationRepository.GetAll(doctorTimeslotPair.Key).OrderBy(examination => examination.Start)
                    .Where(examination => examination.Start > DateTime.Now && !examination.Urgent).ToList();

                postponeableExaminations.InsertRange(0, doctorExaminations);
            }
        }

        if (postponeableExaminations.Count > 5)
            postponeableExaminations = postponeableExaminations.Take(5).ToList();

        if (postponeableExaminations.Count == 0)
        {
            MessageBox.Show("There are no examinations to postpone", "Error");
            return;
        }

        var postponeExaminationViewModel = new PostponeExaminationViewModel(postponeableExaminations, doctorFirstFreeTimeslot);
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