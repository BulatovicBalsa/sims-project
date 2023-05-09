﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Hospital.Models.Doctor;
using Hospital.Models.Examination;
using Hospital.Models.Patient;
using Hospital.Repositories.Examinaton;
using Hospital.Services;

namespace Hospital.ViewModels.Nurse.UrgentExaminations;

public class PostponeExaminationViewModel : ViewModelBase
{
    private readonly Dictionary<Doctor, DateTime> _doctorEarliestFreeTimeslot;
    private readonly ExaminationRepository _examinationRepository;
    private readonly ExaminationService _examinationService;
    private Examination? _selectedExamination;
    private readonly Patient _selectedPatient;

    public PostponeExaminationViewModel()
    {
        // dummy constructor
    }

    public PostponeExaminationViewModel(List<Examination> examinations,
        SortedDictionary<DateTime, Doctor> earliestFreeTimeslotDoctor, Patient selectedPatient)
    {
        Examinations = examinations;
        _selectedExamination = null;
        _doctorEarliestFreeTimeslot = earliestFreeTimeslotDoctor.ToDictionary(pair => pair.Value, pair => pair.Key);
        _examinationRepository = new ExaminationRepository();
        _examinationService = new ExaminationService();
        _selectedPatient = selectedPatient;

        PostponeExaminationCommand =
            new ViewModelCommand(ExecutePostponeExaminationCommand, CanExecutePostponeExaminationCommand);
        CloseDialogCommand = new ViewModelCommand(ExecuteCloseDialogCommand);
    }

    public List<Examination> Examinations { get; }

    public Examination? SelectedExamination
    {
        get => _selectedExamination;
        set
        {
            _selectedExamination = value;
            OnPropertyChanged(nameof(SelectedExamination));
        }
    }

    public ICommand PostponeExaminationCommand { get; }
    public ICommand CloseDialogCommand { get; }

    public event Action<bool, DateTime?, Doctor?>? DialogClosed;

    private void ExecutePostponeExaminationCommand(object obj)
    {
        var previousStart = SelectedExamination.Start;

        if (_examinationService.IsPatientBusy(_selectedPatient, previousStart))
        {
            CloseDialog(false, previousStart, SelectedExamination.Doctor);
            return;
        }

        SelectedExamination.Start = _doctorEarliestFreeTimeslot[SelectedExamination.Doctor];
        _examinationRepository.Update(SelectedExamination, false);

        CloseDialog(false, previousStart, SelectedExamination.Doctor);
    }

    private bool CanExecutePostponeExaminationCommand(object obj)
    {
        return SelectedExamination != null;
    }

    private void ExecuteCloseDialogCommand(object obj)
    {
        CloseDialog(true, null, null);
    }

    private void CloseDialog(bool cancelled, DateTime? newTimeslot, Doctor? freeDoctor)
    {
        Application.Current.Windows[1]?.Close();
        DialogClosed?.Invoke(cancelled, newTimeslot, freeDoctor);
    }
}
