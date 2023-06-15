using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Hospital.Core.PatientHealthcare.Models;
using Hospital.GUI.ViewModels.PatientHealthcare;

namespace Hospital.GUI.Views.PatientHealthcare;

public partial class MedicalRecordPage : Page
{
    private readonly MedicalRecordViewModel _viewModel;

    public MedicalRecordPage(Patient patient, bool isEditable)
    {
        _viewModel = new MedicalRecordViewModel(patient);
        DataContext = _viewModel;

        InitializeComponent();

        ConfigPage(patient);
        ConfigEditableGuiElements(isEditable);
    }

    private void ConfigEditableGuiElements(bool isEditable)
    {
        LogicalTreeHelper.GetChildren(MedicalRecordGrid)
            .OfType<StackPanel>()
            .ToList()
            .ForEach(sp => updateStackPanelButtonsVisibility(sp, isEditable));

        LogicalTreeHelper.GetChildren(MedicalRecordGrid)
            .OfType<TextBox>()
            .ToList()
            .ForEach(tb => tb.IsReadOnly = !isEditable);

        LogicalTreeHelper.GetChildren(MedicalRecordGrid)
            .OfType<Button>()
            .ToList()
            .ForEach(btn => btn.Visibility = isEditable ? Visibility.Visible : Visibility.Collapsed);
    }

    private void updateStackPanelButtonsVisibility(StackPanel stackPanel, bool isEditable)
    {
        var buttons = stackPanel.Children.OfType<Button>();

        foreach (var button in buttons) button.Visibility = isEditable ? Visibility.Visible : Visibility.Collapsed;
    }


    private void ConfigPage(Patient patient)
    {
        Title = $"{patient.FirstName} {patient.LastName}";
    }

    private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
    {
        e.Handled = !isTextAllowed(e.Text);
    }

    private bool isTextAllowed(string text)
    {
        var regex = new Regex("[^0-9]+");
        return !regex.IsMatch(text);
    }
}