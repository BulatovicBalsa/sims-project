using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Hospital.Coordinators;
using Hospital.Models.Doctor;
using Hospital.Models.Patient;

namespace Hospital.ViewModels;

public class CreateReferralViewModel : ViewModelBase
{
    private readonly DoctorService _doctorService = new();
    private ObservableCollection<Doctor> _doctors;

    private Doctor? _selectedDoctor;

    private string? _selectedSpecialization;

    private ObservableCollection<string> _specializations;

    private bool isChanged = false;

    public CreateReferralViewModel(Referral? referralToCreate)
    {
        ReferralToCreate = referralToCreate;
        Doctors = new ObservableCollection<Doctor>(_doctorService.GetAll());
        Specializations = new ObservableCollection<string>(_doctorService.GetAllSpecializations());
        CreateReferralCommand = new RelayCommand<Window>(CreateReferral);
    }

    public Doctor? SelectedDoctor
    {
        get => _selectedDoctor;
        set
        {
            _selectedDoctor = value;
            OnPropertyChanged(nameof(SelectedDoctor));

            if (isChanged)
            {
                isChanged = false;
                return;
            }
            if (!isChanged)
            {
                isChanged = true;
                SelectedSpecialization = null;
            }
        }
    }

    public string? SelectedSpecialization
    {
        get => _selectedSpecialization;
        set
        {
            _selectedSpecialization = value;
            OnPropertyChanged(nameof(SelectedSpecialization));

            if (isChanged)
            {
                isChanged = false;
                return;
            }
            if (!isChanged)
            {
                isChanged = true;
                SelectedDoctor = null;
            }
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