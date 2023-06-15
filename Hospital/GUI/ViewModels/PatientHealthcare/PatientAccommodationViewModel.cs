using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Hospital.Core.PatientHealthcare.Models;
using Hospital.Core.PatientHealthcare.Services;
using Hospital.Core.PhysicalAssets.Models;

namespace Hospital.GUI.ViewModels.PatientHealthcare;

public class PatientAccommodationViewModel : ViewModelBase
{
    private readonly HospitalTreatmentReferralService _hospitalTreatmentReferralService;
    private readonly PatientService _patientService;
    private ObservableCollection<Patient> _accommodablePatients;
    private ObservableCollection<Room>? _availableRooms;
    private Patient? _selectedPatient;
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
                AvailableRooms = new ObservableCollection<Room>(_hospitalTreatmentReferralService.GetAvailableRooms());
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

    private void ExecuteAccommodatePatientCommand(object obj)
    {
        var unusedPatientReferral = SelectedPatient!.GetFirstUnusedHospitalTreatmentReferral();
        unusedPatientReferral.Accommodate(SelectedRoom!.Id);

        _patientService.UpdateHospitalTreatmentReferral(SelectedPatient, unusedPatientReferral);

        MessageBox.Show("Patient successfully accommodated.");
        ResetInput();
    }

    private bool CanExecuteAccommodatePatientCommand(object obj)
    {
        return SelectedPatient != null && SelectedRoom != null;
    }

    private void ResetInput()
    {
        AccommodablePatients = new ObservableCollection<Patient>(_patientService.GetAllAccommodablePatients());
        SelectedPatient = null;
        AvailableRooms = null;
        SelectedRoom = null;
    }
}