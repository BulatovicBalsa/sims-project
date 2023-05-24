using System;
using Hospital.Models.Patient;
using Hospital.Repositories.Patient;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Hospital.Models.Examination;
using Hospital.Services;

namespace Hospital.ViewModels.Nurse.Referrals;

public class PatientReferralsViewModel : ViewModelBase
{
    private readonly PatientRepository _patientRepository;
    private ObservableCollection<Patient> _patients;
    private Patient? _selectedPatient;
    private ObservableCollection<Referral>? _patientReferrals;
    private Referral? _selectedReferral;
    private DateTime? _selectedDate;
    private TimeOnly? _selectedTime;
    private ObservableCollection<TimeOnly>? _possibleTimeslots;
    private readonly TimeslotService _timeslotService;
    private readonly ExaminationService _examinationService;

    public PatientReferralsViewModel()
    {
        _patientRepository = new PatientRepository();
        _patients = new ObservableCollection<Patient>(_patientRepository.GetAll());
        _selectedPatient = null;
        _patientReferrals = null;
        _selectedReferral = null;
        _selectedDate = null;
        _selectedTime = null;
        _possibleTimeslots = null;
        _timeslotService = new TimeslotService();
        _examinationService = new ExaminationService();

        UseReferralCommand = new ViewModelCommand(ExecuteUseReferralCommand, CanExecuteUseReferralCommand);
    }

    public Patient? SelectedPatient
    {
        get => _selectedPatient;
        set
        {
            _selectedPatient = value;
            OnPropertyChanged(nameof(SelectedPatient));
            PatientReferrals = new ObservableCollection<Referral>(SelectedPatient.Referrals);
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

    public Referral? SelectedReferral
    {
        get => _selectedReferral;
        set
        {
            _selectedReferral = value;

            if (SelectedReferral.Doctor == null)
                SelectedReferral.AssignDoctor();

            OnPropertyChanged(nameof(SelectedReferral));
        }
    }

    public ObservableCollection<Referral>? PatientReferrals
    {
        get => _patientReferrals;
        set
        {
            _patientReferrals = value;
            OnPropertyChanged(nameof(PatientReferrals));
        }
    }

    public DateTime? SelectedDate
    {
        get => _selectedDate;
        set
        {
            _selectedDate = value;
            OnPropertyChanged(nameof(SelectedDate));
            PossibleTimeslots = new ObservableCollection<TimeOnly>(_timeslotService.GetFreeTimeslotsForDate(SelectedReferral.Doctor, (DateTime)SelectedDate));
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

    public ObservableCollection<TimeOnly>? PossibleTimeslots
    {
        get => _possibleTimeslots;
        set
        {
            _possibleTimeslots = value;
            OnPropertyChanged(nameof(PossibleTimeslots));
        }
    }

    public ICommand UseReferralCommand { get; }

    private void ExecuteUseReferralCommand(object obj)
    {
        var examinationStart = new DateTime(SelectedDate.Value.Year, SelectedDate.Value.Month, SelectedDate.Value.Day, SelectedTime.Value.Hour, SelectedTime.Value.Minute, 0);
        var newExamination = new Examination(SelectedReferral.Doctor, SelectedPatient, false, examinationStart, null);
        try
        {
            _examinationService.AddExamination(newExamination, false);
        }
        catch (Exception)
        {
            MessageBox.Show("Selected timeslot is already occupied", "Error");
            return;
        }

        SelectedPatient.RemoveReferral(SelectedReferral);
        _patientRepository.Update(SelectedPatient);

        MessageBox.Show("Examination successfully created", "Success");
        ResetInput();
    }

    private void ResetInput()
    {
        SelectedReferral = null;
        SelectedPatient = null;
        SelectedDate = null;
        SelectedTime = null;
    }

    private bool CanExecuteUseReferralCommand(object obj)
    {
        return SelectedPatient != null && SelectedReferral != null && SelectedDate != null && SelectedTime != null;
    }

}

