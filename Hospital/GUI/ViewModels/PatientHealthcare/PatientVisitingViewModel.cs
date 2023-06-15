using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Hospital.Filter.Nurse;
using Hospital.GUI.Views.PatientHealthcare;
using Hospital.PatientHealthcare.Models;
using Hospital.PatientHealthcare.Services;
using Hospital.PhysicalAssets.Models;
using Hospital.PhysicalAssets.Services;

namespace Hospital.GUI.ViewModels.PatientHealthcare;

public class PatientVisitingViewModel : ViewModelBase
{
    private readonly List<Patient> _hospitalizedPatientsBase;
    private readonly IPatientFilter _patientAccommodationRoomFilter;
    private readonly IPatientFilter _patientNameFilter;
    private readonly PatientService _patientService;
    private readonly RoomFilterService _roomFilterService;
    private ObservableCollection<Room> _allRooms;
    private string _filterName;
    private Room? _filterRoom;
    private ObservableCollection<Patient> _hospitalizedPatients;
    private Patient? _selectedPatient;

    public PatientVisitingViewModel()
    {
        _patientService = new PatientService();
        _roomFilterService = new RoomFilterService();
        _hospitalizedPatientsBase = _patientService.GetAllHospitalizedPatients();
        _allRooms = new ObservableCollection<Room>(_roomFilterService.GetRoomsForAccommodation());
        _hospitalizedPatients = new ObservableCollection<Patient>(_hospitalizedPatientsBase);
        _selectedPatient = null;
        VisitPatientCommand = new ViewModelCommand(ExecuteVisitPatientCommand, CanExecuteVisitPatientCommand);
        _filterName = "";
        _filterRoom = null;
        ResetFilterCommand = new ViewModelCommand(ExecuteResetFilterCommand);
        _patientNameFilter = new PatientNameFilter();
        _patientAccommodationRoomFilter = new PatientAccommodationRoomFilter();
    }

    public ObservableCollection<Patient> HospitalizedPatients
    {
        get => _hospitalizedPatients;
        set
        {
            _hospitalizedPatients = value;
            OnPropertyChanged(nameof(HospitalizedPatients));
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

    public string FilterName
    {
        get => _filterName;
        set
        {
            _filterName = value;
            OnPropertyChanged(nameof(FilterName));
            DualFilter(_patientNameFilter, FilterName, _patientAccommodationRoomFilter, FilterRoom?.Id ?? null);
        }
    }

    public ObservableCollection<Room> AllRooms
    {
        get => _allRooms;
        set
        {
            _allRooms = value;
            OnPropertyChanged(nameof(AllRooms));
        }
    }

    public Room? FilterRoom
    {
        get => _filterRoom;
        set
        {
            _filterRoom = value;
            OnPropertyChanged(nameof(FilterRoom));

            if (value != null)
                DualFilter(_patientAccommodationRoomFilter, FilterRoom!.Id, _patientNameFilter, FilterName);
        }
    }

    public ICommand VisitPatientCommand { get; }
    public ICommand ResetFilterCommand { get; }

    private void DualFilter(IPatientFilter filter, object? filterProperty, IPatientFilter? otherFilter = null,
        object? otherProperty = null, bool fromScratch = false)
    {
        var basePatients = _hospitalizedPatientsBase;

        if (!fromScratch && otherProperty != null)
        {
            DualFilter(otherFilter!, otherProperty, fromScratch: true);
            basePatients = HospitalizedPatients.ToList();
        }

        var matchingPatients = filter.Filter(basePatients, filterProperty!);
        HospitalizedPatients = new ObservableCollection<Patient>(matchingPatients);
    }

    private void ExecuteVisitPatientCommand(object obj)
    {
        var visitingDialogViewModel = new VisitingDialogViewModel(SelectedPatient!.Id);
        var visitingDialog = new VisitingDialogView
        {
            DataContext = visitingDialogViewModel
        };

        visitingDialog.ShowDialog();
    }

    private bool CanExecuteVisitPatientCommand(object obj)
    {
        return SelectedPatient != null;
    }

    private void ExecuteResetFilterCommand(object obj)
    {
        _filterName = "";
        OnPropertyChanged(nameof(FilterName));
        _filterRoom = null;
        OnPropertyChanged(nameof(FilterRoom));
        HospitalizedPatients = new ObservableCollection<Patient>(_hospitalizedPatientsBase);
    }
}