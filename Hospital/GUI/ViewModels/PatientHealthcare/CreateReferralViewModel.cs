using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Hospital.Core.PatientHealthcare.Models;
using Hospital.Core.Workers.Models;
using Hospital.Core.Workers.Services;

namespace Hospital.GUI.ViewModels.PatientHealthcare;

public class CreateReferralViewModel : ViewModelBase
{
    private readonly DoctorService _doctorService = new();
    private ObservableCollection<Doctor> _doctors;

    private bool _isSelectedItemChanged;

    private Doctor? _selectedDoctor;

    private string? _selectedSpecialization;

    private ObservableCollection<string> _specializations;

    public CreateReferralViewModel(Referral? referralToCreate)
    {
        _doctors = new ObservableCollection<Doctor>(_doctorService.GetAll());
        _specializations = new ObservableCollection<string>(_doctorService.GetAllSpecializations());

        ReferralToCreate = referralToCreate;
        CreateReferralCommand = new RelayCommand<Window>(CreateReferral);
    }

    public Doctor? SelectedDoctor
    {
        get => _selectedDoctor;
        set
        {
            _selectedDoctor = value;
            OnPropertyChanged(nameof(SelectedDoctor));

            TrySwitchOppositeFieldToNull(true);
        }
    }

    public string? SelectedSpecialization
    {
        get => _selectedSpecialization;
        set
        {
            _selectedSpecialization = value;
            OnPropertyChanged(nameof(SelectedSpecialization));

            TrySwitchOppositeFieldToNull(false);
        }
    }

    public ObservableCollection<Doctor> Doctors
    {
        get => _doctors;
        set
        {
            _doctors = value;
            OnPropertyChanged(nameof(Doctors));
        }
    }

    public ObservableCollection<string> Specializations
    {
        get => _specializations;
        set
        {
            _specializations = value;
            OnPropertyChanged(nameof(Specializations));
        }
    }

    public Referral? ReferralToCreate { get; set; }

    public ICommand CreateReferralCommand { get; set; }

    private void TrySwitchOppositeFieldToNull(bool doesDoctorCalled)
    {
        if (_isSelectedItemChanged)
        {
            _isSelectedItemChanged = false;
            return;
        }

        _isSelectedItemChanged = true;
        if (doesDoctorCalled) SelectedSpecialization = null;
        else SelectedDoctor = null;
    }

    private void CreateReferral(Window window)
    {
        if (SelectedDoctor is null && SelectedSpecialization is null)
        {
            MessageBox.Show("You must enter Specializations or Doctor");
            return;
        }

        var referralDto = SelectedDoctor is null
            ? new Referral(SelectedSpecialization!)
            : new Referral(SelectedDoctor!);

        ReferralToCreate?.DeepCopy(referralDto);
        MessageBox.Show("Succeed");
        window.DialogResult = true;
    }
}