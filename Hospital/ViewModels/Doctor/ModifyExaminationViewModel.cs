﻿using GalaSoft.MvvmLight.Command;
using Hospital.Exceptions;
using Hospital.Models.Doctor;
using Hospital.Models.Examination;
using Hospital.Models.Manager;
using Hospital.Models.Patient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Hospital.Services;
using Hospital.Services.Manager;

namespace Hospital.ViewModels;

public class ModifyExaminationViewModel : ViewModelBase
{
    private readonly Doctor _doctor;
    private readonly ObservableCollection<Examination> _examinationCollection;
    private readonly ExaminationService _examinationService = new();
    private readonly Examination? _examinationToChange;
    private readonly bool _isUpdate;
    private readonly PatientService _patientService = new();
    private readonly RoomFilterService _roomFilterService = new();

    private string _buttonContent;

    private bool _isOperation;

    private ObservableCollection<Patient> _patients;

    private ObservableCollection<TimeOnly> _possibleTimes;

    private DateTime? _selectedDate;

    private Patient? _selectedPatient;

    private Room? _selectedRoom;

    private TimeOnly? _selectedTime;

    public ModifyExaminationViewModel(Doctor doctor, ObservableCollection<Examination> examinationCollection,
        Examination? examinationToChange = null, bool isForTenDays = false, Patient patientInTenDays = null)
    {
        _isUpdate = examinationToChange is not null;
        _doctor = doctor;
        _examinationCollection = examinationCollection;
        _examinationToChange = examinationToChange;

        Patients = new ObservableCollection<Patient>(_patientService.GetAllPatients());
        PossibleTimes = new ObservableCollection<TimeOnly>(GetPossibleTimes());
        Rooms = new ObservableCollection<Room>(_roomFilterService.GetRoomsForExamination());

        ModifyExaminationCommand = new RelayCommand<Window>(ModifyExamination);
        FillForm();

        if (!isForTenDays) return;
        SelectedDate = DateTime.Today.AddDays(10);
        SelectedPatient = patientInTenDays;
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

    public DateTime? SelectedDate
    {
        get => _selectedDate;
        set
        {
            _selectedDate = value;
            OnPropertyChanged(nameof(SelectedDate));
        }
    }

    public Patient? SelectedPatient
    {
        get => _selectedPatient;
        set
        {
            _selectedPatient = value;
            OnPropertyChanged(nameof(SelectedPatient));
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

    public string ButtonContent
    {
        get => _buttonContent;
        set
        {
            _buttonContent = value;
            OnPropertyChanged(nameof(ButtonContent));
        }
    }

    public TimeOnly? SelectedTime
    {
        get => _selectedTime;
        set
        {
            _selectedTime = value;
            OnPropertyChanged(nameof(SelectedTime));
        }
    }

    public ObservableCollection<TimeOnly> PossibleTimes
    {
        get => _possibleTimes;
        set
        {
            _possibleTimes = value;
            OnPropertyChanged(nameof(PossibleTimes));
        }
    }

    public ObservableCollection<Room> Rooms { get; set; }

    public Room? SelectedRoom
    {
        get => _selectedRoom;
        set
        {
            _selectedRoom = value;
            OnPropertyChanged(nameof(SelectedRoom));
        }
    }

    public ICommand ModifyExaminationCommand { get; set; }

    private void FillForm()
    {
        SelectedDate = _examinationToChange?.Start ?? DateTime.Now;
        IsOperation = _examinationToChange is not null && _examinationToChange.IsOperation;
        SelectedTime = _examinationToChange is null
            ? null
            : TimeOnly.FromTimeSpan(_examinationToChange.Start.TimeOfDay);
        SelectedPatient = _examinationToChange?.Patient;
        SelectedRoom = _examinationToChange?.Room;
        ButtonContent = _examinationToChange is null ? "Create" : "Update";
    }

    private void ModifyExamination(Window window)
    {
        var createdExamination = CreateExaminationFromForm();
        if (createdExamination is null) return;

        try
        {
            if (_isUpdate)
            {
                UpdateExamination(createdExamination);
            }
            else
            {
                _examinationService.AddExamination(createdExamination, false);
                _examinationCollection.Add(createdExamination);
            }
        }
        catch (Exception ex)
        {
            if (ex is DoctorBusyException or PatientBusyException or RoomBusyException)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }

        window.DialogResult = true;
    }

    private Examination? CreateExaminationFromForm()
    {
        if (SelectedPatient == null
            || SelectedDate == null
            || SelectedTime == null
            || SelectedRoom == null)
        {
            MessageBox.Show("You must select patient, date, time, and room");
            return null;
        }

        var startDate = CreateDateFromForm();

        var createdExamination = _isUpdate ? _examinationToChange : new Examination();
        createdExamination?.Update(new UpdateExaminationDto(startDate, IsOperation, SelectedRoom, SelectedPatient,
            _doctor));
        return createdExamination;
    }

    private DateTime CreateDateFromForm()
    {
        var startTime = SelectedTime.GetValueOrDefault();
        var startDate = SelectedDate.GetValueOrDefault();
        startDate = startDate.Add(startTime.ToTimeSpan());
        return startDate;
    }

    private void UpdateExamination(Examination examination)
    {
        if (_examinationToChange != null) examination.Id = _examinationToChange.Id;
        else throw new InvalidOperationException("examination to change can't be null");
        _examinationService.UpdateExamination(examination, false);
        _examinationCollection.Clear();
        _examinationService.GetExaminationsForNextThreeDays(_doctor)
            .ForEach(examinationInRange => _examinationCollection.Add(examinationInRange));
    }

    private List<TimeOnly> GetPossibleTimes()
    {
        var possibleTimes = new List<TimeOnly>();

        for (var hour = 0; hour <= 23; hour++)
        for (var minute = 0; minute < 60; minute += 15)
            possibleTimes.Add(new TimeOnly(hour, minute));
        return possibleTimes;
    }
}