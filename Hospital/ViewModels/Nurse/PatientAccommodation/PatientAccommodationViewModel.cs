using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Hospital.Models.Manager;
using Hospital.Models.Patient;
using Hospital.Services;

namespace Hospital.ViewModels.Nurse.PatientAccommodation;
public class PatientAccommodationViewModel : ViewModelBase
{
    private readonly PatientService _patientService;
    private readonly HospitalTreatmentReferralService _hospitalTreatmentReferralService;
    private ObservableCollection<Patient> _accommodablePatients;
    private Patient? _selectedPatient;
    private ObservableCollection<Room>? _availableRooms;
    private Room? _selectedRoom;

    public PatientAccommodationViewModel()
    {
        _patientService = new PatientService();
        _hospitalTreatmentReferralService = new HospitalTreatmentReferralService();
        _accommodablePatients = new ObservableCollection<Patient>(_patientService.GetAllAccommodablePatients());
        _selectedPatient = null;
        _availableRooms = null;
        _selectedRoom = null;
        AccommodatePatientCommand =
            new ViewModelCommand(ExecuteAccommodatePatientCommand, CanExecuteAccommodatePatientCommand);
    }

    public ObservableCollection<Patient> AccommodablePatients
    {
        get => _accommodablePatients;
        set
        {
            _accommodablePatients = value;
            OnPropertyChanged(nameof(AccommodablePatients));
        }
    }

    public Patient? SelectedPatient
    {
        get => _selectedPatient;
        set
        {
            _selectedPatient = value;
            OnPropertyChanged(nameof(SelectedPatient));

            if (value != null)
            {
                AvailableRooms = new ObservableCollection<Room>(_hospitalTreatmentReferralService.GetAvailableRooms());
            }
        }
    }

    public ObservableCollection<Room>? AvailableRooms
    {
        get => _availableRooms;
        set
        {
            _availableRooms = value;
            OnPropertyChanged(nameof(AvailableRooms));
        }
    }

    public Room? SelectedRoom
    {
        get => _selectedRoom;
        set
        {
            _selectedRoom = value;
            OnPropertyChanged(nameof(SelectedRoom));
        }
    }

    public ICommand AccommodatePatientCommand { get; }

    public void ExecuteAccommodatePatientCommand(object obj)
    {
        throw new NotImplementedException();
    }

    public bool CanExecuteAccommodatePatientCommand(object obj)
    {
        return SelectedPatient != null && SelectedRoom != null;
    }
}
