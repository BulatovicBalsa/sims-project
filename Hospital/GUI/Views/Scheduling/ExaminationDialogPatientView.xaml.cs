using System.Windows;
using Hospital.Core.PatientHealthcare.Models;
using Hospital.Core.Workers.Models;
using Hospital.GUI.ViewModels.PatientHealthcare;

namespace Hospital.GUI.Views.Scheduling;

public partial class ExaminationDialogView : Window
{
    private readonly ExaminationViewModel _viewModel;

    public ExaminationDialogView(Patient patient, PatientViewModel patientViewModel, bool isUpdate,
        Examination examination = null, Doctor doctor = null)
    {
        InitializeComponent();

        _viewModel = examination != null
            ? new ExaminationViewModel(patient, patientViewModel, examination)
            : new ExaminationViewModel(patient, patientViewModel, doctor: doctor);

        _viewModel.IsUpdate = isUpdate;
        DataContext = _viewModel;
    }

    private void BtnCancel_Click(object sender, RoutedEventArgs e)
    {
        var dialogWindow = GetWindow(this);
        dialogWindow.Close();
    }

    private void BtnSave_Click(object sender, RoutedEventArgs e)
    {
        _viewModel.SaveCommand.Execute(null);
    }
}