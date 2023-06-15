using System;
using System.Collections.ObjectModel;
using System.Windows;
using Hospital.GUI.ViewModels.PatientHealthcare;
using Hospital.PatientHealthcare.Models;
using Hospital.Workers.Models;

namespace Hospital.GUI.Views.PatientHealthcare;

public partial class ModifyExaminationDialog : Window
{
    public ModifyExaminationDialog(Doctor doctor,
        ObservableCollection<Examination> examinationCollection,
        Examination examinationToChange = null, DateTime? examinationDate = null,
        Patient patientInTenDays = null)
    {
        DataContext = new ModifyExaminationViewModel(doctor, examinationCollection, examinationToChange,
            examinationDate, patientInTenDays);
        InitializeComponent();
        ConfigWindow();
    }

    private void ConfigWindow()
    {
        WindowStartupLocation = WindowStartupLocation.CenterScreen;
        SizeToContent = SizeToContent.WidthAndHeight;
    }
}