using System.Collections.ObjectModel;
using Hospital.Models.Patient;
using Hospital.Services;

namespace Hospital.ViewModels.Nurse.PatientAccommodation;
public class PatientAccommodationViewModel : ViewModelBase
{
    private readonly PatientService _patientService;
    private ObservableCollection<Patient> _accommodablePatients;
    private Patient? _selectedPatient;

    public PatientAccommodationViewModel()
    {
        _patientService = new PatientService();
        _accommodablePatients = new ObservableCollection<Patient>(_patientService.GetAllAccommodablePatients());
        _selectedPatient = null;
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

    public Patient SelectedPatient
    {
        get => _selectedPatient;
        set
        {
            _selectedPatient = value;
            OnPropertyChanged(nameof(SelectedPatient));
        }
    }
}
