using Hospital.Models.Patient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.VisualBasic;
using Hospital.Coordinators;
using System.Text.RegularExpressions;
using Hospital.ViewModels;

namespace Hospital.Views
{
    public partial class MedicalRecordPage : Page
    {
        private MedicalRecordPageViewModel _viewModel;

        public MedicalRecordPage(Patient patient, bool isEditable)
        {
            _viewModel = new MedicalRecordPageViewModel(patient);
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

            foreach (var button in buttons)
            {
                button.Visibility = isEditable ? Visibility.Visible : Visibility.Collapsed;
            }
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
            Regex regex = new Regex("[^0-9]+");
            return !regex.IsMatch(text);
        }
    }
}
