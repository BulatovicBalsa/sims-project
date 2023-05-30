using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Hospital.Models.Patient;
using Hospital.Services;
using Microsoft.VisualBasic;

namespace Hospital.ViewModels;

public class CreateHospitalTreatmentReferralViewModel : ViewModelBase
{
    private readonly Patient _patientOnExamination;
    private readonly PatientService _patientService = new();

    private ObservableCollection<string> _additionalTests = new();
    private int _duration;

    private string? _selectedTest;

    public CreateHospitalTreatmentReferralViewModel(Patient patientOnExamination,
        HospitalTreatmentReferral referralToModify)
    {
        _patientOnExamination = patientOnExamination;
        Referral = referralToModify;
        Duration = 1;
        AdditionalTests = new ObservableCollection<string>();

        AddReferralCommand = new RelayCommand<Window>(AddReferral);
        AddAdditionalTestCommand = new RelayCommand(AddAdditionalTest);
        DeleteAdditionalTestCommand = new RelayCommand(DeleteAdditionalTest);
    }

    public int Duration
    {
        get => _duration;
        set
        {
            _duration = value;
            OnPropertyChanged(nameof(Duration));
        }
    }

    public ObservableCollection<string> AdditionalTests
    {
        get => _additionalTests;
        set
        {
            _additionalTests = value;
            OnPropertyChanged(nameof(AdditionalTests));
        }
    }

    public string? SelectedTest
    {
        get => _selectedTest;
        set
        {
            _selectedTest = value;
            OnPropertyChanged(nameof(SelectedTest));
        }
    }

    public HospitalTreatmentReferral Referral { get; set; }

    public ICommand AddReferralCommand { get; set; }
    public ICommand AddAdditionalTestCommand { get; set; }
    public ICommand DeleteAdditionalTestCommand { get; set; }

    private void DeleteAdditionalTest()
    {
        if (SelectedTest is null)
        {
            MessageBox.Show("You must select additional test in order to delete it", "", MessageBoxButton.OK,
                MessageBoxImage.Error);
            return;
        }

        AdditionalTests.Remove(SelectedTest);
    }

    private void AddAdditionalTest()
    {
        var testToAdd = Interaction.InputBox("Insert additional test: ", "Add additional test");
        AdditionalTests.Add(testToAdd);
    }

    private void AddReferral(Window window)
    {
        Referral.AdditionalTests = AdditionalTests.ToList();
        Referral.Duration = Duration;
        _patientOnExamination.HospitalTreatmentReferrals.Add(Referral);
        _patientService.UpdatePatient(_patientOnExamination);
        MessageBox.Show("Succeed");
        window.DialogResult = true;
    }
}