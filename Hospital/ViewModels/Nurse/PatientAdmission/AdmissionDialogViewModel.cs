using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Hospital.Models.Examination;
using Hospital.Models.Patient;
using Hospital.Repositories.Examinaton;
using Hospital.Repositories.Patient;

namespace Hospital.ViewModels.Nurse.PatientAdmission;

public class AdmissionDialogViewModel : ViewModelBase
{
    private readonly Examination _examination;
    private readonly ExaminationRepository _examinationRepository;
    private readonly PatientRepository _patientRepository;
    private readonly Patient _selectedPatient;
    private string _allergies;
    private string _medicalHistory;
    private string _symptoms;

    public AdmissionDialogViewModel()
    {
        // dummy constructor
    }

    public AdmissionDialogViewModel(PatientRepository patientRepository, Patient selectedPatient,
        Examination examination)
    {
        _examinationRepository = new ExaminationRepository();
        _patientRepository = patientRepository;
        _selectedPatient = selectedPatient;
        _examination = examination;

        Allergies = GetCommaSeparatedString(selectedPatient.MedicalRecord.Allergies.Conditions);
        MedicalHistory = GetCommaSeparatedString(selectedPatient.MedicalRecord.MedicalHistory.Conditions);

        EndAdmissionCommand = new ViewModelCommand(ExecuteEndAdmissionCommand, CanExecuteEndAdmissionCommand);
        CancelCommand = new ViewModelCommand(ExecuteCancelCommand);
    }

    public string Allergies
    {
        get => _allergies;
        set
        {
            _allergies = value;
            OnPropertyChanged(nameof(Allergies));
        }
    }

    public string MedicalHistory
    {
        get => _medicalHistory;
        set
        {
            _medicalHistory = value;
            OnPropertyChanged(nameof(MedicalHistory));
        }
    }

    public string Symptoms
    {
        get => _symptoms;
        set
        {
            _symptoms = value;
            OnPropertyChanged(nameof(Symptoms));
        }
    }

    public ICommand EndAdmissionCommand { get; }
    public ICommand CancelCommand { get; }

    private void ExecuteEndAdmissionCommand(object obj)
    {
        UpdateMedicalRecord();
        SaveExamination();

        CloseDialog();
    }

    private void UpdateMedicalRecord()
    {
        _selectedPatient.MedicalRecord.MedicalHistory.Conditions = SplitByComma(MedicalHistory);
        _selectedPatient.MedicalRecord.Allergies.Conditions = SplitByComma(Allergies);

        _patientRepository.Update(_selectedPatient);
    }

    private void SaveExamination()
    {
        _examination.Anamnesis = Symptoms;
        _examination.Admissioned = true;

        if (_examination.Start == DateTime.MinValue)
        {
            _examinationRepository.Add(_examination, false);
        }
        else
        {
            _examinationRepository.Update(_examination, false);
        }
    }

    private bool CanExecuteEndAdmissionCommand(object obj)
    {
        return !string.IsNullOrEmpty(Symptoms);
    }

    private void ExecuteCancelCommand(object obj)
    {
        CloseDialog();
    }

    private string GetCommaSeparatedString(IEnumerable<string> words)
    {
        return string.Join(", ", words);
    }

    private List<string> SplitByComma(string commaSeparated)
    {
        return commaSeparated.Split(", ").ToList();
    }

    private void CloseDialog()
    {
        Application.Current.Windows[1]?.Close();
    }
}