using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Hospital.Core.Notifications.Models;
using Hospital.Core.Notifications.Services;
using Hospital.Core.PatientHealthcare.Models;
using Hospital.Core.PatientHealthcare.Repositories;
using Hospital.Core.PatientHealthcare.Services;
using Hospital.Core.Workers.Models;

namespace Hospital.GUI.ViewModels.Scheduling;

public class PostponeExaminationViewModel : ViewModelBase
{
    private readonly Dictionary<Doctor, DateTime> _doctorEarliestFreeTimeslot;
    private readonly ExaminationRepository _examinationRepository;
    private readonly ExaminationService _examinationService;
    private readonly NotificationService _notificationService;
    private readonly Patient _selectedPatient;
    private Examination? _selectedExamination;

    public PostponeExaminationViewModel()
    {
        // dummy constructor
    }

    public PostponeExaminationViewModel(List<Examination> examinations,
        SortedDictionary<DateTime, Doctor> earliestFreeTimeslotDoctor,
       Patient selectedPatient)
    {
        Examinations = examinations;
        _selectedExamination = null;
        _doctorEarliestFreeTimeslot = earliestFreeTimeslotDoctor.ToDictionary(pair => pair.Value, pair => pair.Key);
        _examinationRepository = ExaminationRepository.Instance;
        _examinationService = new ExaminationService();
        _notificationService = new NotificationService();
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

        PostponeExamination();
        SendNotifications(SelectedExamination);

        CloseDialog(false, previousStart, SelectedExamination.Doctor);
    }

    private void PostponeExamination()
    {
        SelectedExamination.Start = _doctorEarliestFreeTimeslot[SelectedExamination.Doctor];
        _examinationRepository.Update(SelectedExamination, false);
    }

    private void SendNotifications(Examination examination)
    {
        var patientNotification = new Notification(examination.Patient.Id,
            $"Examination {examination.Id} postponed to {examination.Start}");
        var doctorNotification = new Notification(examination.Doctor.Id,
            $"Examination {examination.Id} postponed to {examination.Start}");

        _notificationService.Send(patientNotification);
        _notificationService.Send(doctorNotification);
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