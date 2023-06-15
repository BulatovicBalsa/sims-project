﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Hospital.Core.Notifications.Services;
using Hospital.Core.PatientHealthcare.Models;
using Hospital.Core.PatientHealthcare.Repositories;
using Hospital.Core.PatientHealthcare.Services;
using Hospital.Core.Scheduling.Services;
using Hospital.Core.Workers.Models;
using Hospital.Core.Workers.Services;
using Hospital.GUI.Views.Scheduling;
using Hospital.Injectors;
using Hospital.Notifications.Models;
using Hospital.Serialization;
using Hospital.Workers.Repositories;

namespace Hospital.GUI.ViewModels.Scheduling;

public class UrgentExaminationsViewModel : ViewModelBase
{
    private readonly DoctorService _doctorService;
    private readonly PatientRepository _patientRepository;
    private readonly ExaminationRepository _examinationRepository;
    private readonly ExaminationService _examinationService;
    private readonly NotificationService _notificationService;
    private readonly TimeslotService _timeslotService;
    private bool _isOperation;

    private Patient? _selectedPatient;
    private string? _selectedSpecialization;

    public UrgentExaminationsViewModel()
    {
        _doctorService = new DoctorService();
        _patientRepository = PatientRepository.Instance;
        _timeslotService = new TimeslotService();
        _examinationRepository = ExaminationRepository.Instance;
        _examinationService = new ExaminationService();
        _notificationService = new NotificationService();

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

        if (ScheduleUrgentExamination(earliestFreeTimeslotDoctors))
            return;

        PostponeExamination(earliestFreeTimeslotDoctors);
    }

    private bool ScheduleUrgentExamination(
        SortedDictionary<DateTime, Doctor> earliestFreeTimeslotDoctors)
    {
        var earliestFreeTimeslotDoctor = earliestFreeTimeslotDoctors.First();
        if (!TimeslotService.IsIn2Hours(earliestFreeTimeslotDoctor.Key)) return false;

        SaveUrgentExamination(false, earliestFreeTimeslotDoctor.Key, earliestFreeTimeslotDoctor.Value);
        return true;
    }

    private void PostponeExamination(SortedDictionary<DateTime, Doctor> earliestFreeTimeslotDoctors)
    {
        var postponableExaminations = _examinationService.GetFivePostponableExaminations(earliestFreeTimeslotDoctors);
        if (postponableExaminations.Count == 0)
        {
            MessageBox.Show("There are no examinations that can be postponed", "Error");
            return;
        }

        var postponeExaminationViewModel =
            new PostponeExaminationViewModel(postponableExaminations, earliestFreeTimeslotDoctors, SelectedPatient);
        postponeExaminationViewModel.DialogClosed += SaveUrgentExamination;

        OpenPostponeExaminationDialog(postponeExaminationViewModel);
    }

    private void SaveUrgentExamination(bool cancelled, DateTime? newTimeslot, Doctor? freeDoctor)
    {
        if (cancelled)
            return;

        if (_examinationService.IsPatientBusy(SelectedPatient, newTimeslot ?? DateTime.MinValue))
        {
            MessageBox.Show("Patient already has an examination at given time", "Error");
            return;
        }

        _examinationRepository.Add(
            new Examination(freeDoctor, SelectedPatient, IsOperation,
                newTimeslot ?? DateTime.MinValue, null, true), false);
        SendDoctorNotification(freeDoctor, newTimeslot ?? DateTime.MinValue);
        MessageBox.Show("Urgent examination successfully created", "Success");
    }

    private void SendDoctorNotification(Doctor doctor, DateTime timeslot)
    {
        var notification = new Notification(doctor.Id, $"New urgent examination scheduled at {timeslot}");
        _notificationService.Send(notification);
    }

    private void OpenPostponeExaminationDialog(PostponeExaminationViewModel viewModel)
    {
        var postponeExaminationDialog = new PostponeExaminationDialogView
        {
            DataContext = viewModel
        };
        postponeExaminationDialog.ShowDialog();
    }

    private bool CanExecuteCreateUrgentExaminationCommand(object obj)
    {
        return SelectedPatient != null && SelectedSpecialization != null;
    }
}