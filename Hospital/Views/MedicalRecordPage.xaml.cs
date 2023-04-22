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

namespace Hospital.Views
{
    public partial class MedicalRecordPage : Page
    {
        private Patient _patient;
        private readonly DoctorCoordinator _doctorCoordinator;

        public MedicalRecordPage(Patient patient, bool isEditable)
        {
            _patient = patient;
            _doctorCoordinator = new DoctorCoordinator();
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
            DataContext = patient.MedicalRecord;
            Title = $"{patient.FirstName} {patient.LastName}";
        }

        private void AddAllergyButton_Click(object sender, RoutedEventArgs e)
        {
            addHealthCondition(HealthConditionType.Allergy);
        }

        private void AddMedicalConditionButton_Click(object sender, RoutedEventArgs e)
        {
            addHealthCondition(HealthConditionType.MedicalCondition);
        }

        private void UpdateAllergyButton_Click(object sender, RoutedEventArgs e)
        {
            updateHealthCondition(HealthConditionType.Allergy);
        }

        private void UpdateMedicalConditionButton_Click(object sender, RoutedEventArgs e)
        {
            updateHealthCondition(HealthConditionType.MedicalCondition);
        }

        private void DeleteMedicalConditionButton_Click(object sender, RoutedEventArgs e)
        {
            deleteHealthCondition(HealthConditionType.MedicalCondition);
        }

        private void DeleteAllergyButton_Click(object sender, RoutedEventArgs e)
        {
            deleteHealthCondition(HealthConditionType.Allergy);
        }

        private void ChangeWeightButton_Click(object sender, RoutedEventArgs e)
        {
            changePhysicalCharacteristic(false);
        }

        private void ChangeHeightButton_Click(object sender, RoutedEventArgs e)
        {
            changePhysicalCharacteristic(true);
        }

        private void addHealthCondition(HealthConditionType conditionType)
        {
            Action<string> medicalRecordOperation = conditionType == HealthConditionType.Allergy ? _patient.MedicalRecord.AddAllergy : _patient.MedicalRecord.AddMedicalConidition;
            var healthConditionListBox = conditionType == HealthConditionType.Allergy ? AllergiesListBox : MedicalHistoryListBox;

            string conditionToAdd = Interaction.InputBox($"Insert {conditionType}: ", $"Add {conditionType}", "");
            try
            {
                medicalRecordOperation(conditionToAdd);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            _doctorCoordinator.UpdatePatient(_patient);
            healthConditionListBox.Items.Refresh();
        }

        private void updateHealthCondition(HealthConditionType conditionType)
        {
            Action<string, string> medicalRecordOperation = conditionType == HealthConditionType.Allergy ? _patient.MedicalRecord.UpdateAllergy : _patient.MedicalRecord.UpdateMedicalCondition;
            var healthConditionListBox = conditionType == HealthConditionType.Allergy ? AllergiesListBox : MedicalHistoryListBox;

            string? selectedCondition = (string)healthConditionListBox.SelectedItem;
            if (selectedCondition == null)
            {
                MessageBox.Show("You must select condition in order to update it");
                return;
            }
            string updatedCondition = Interaction.InputBox($"Update '{selectedCondition}' name: ", $"Update {conditionType}", "");
            try
            {
                medicalRecordOperation(selectedCondition, updatedCondition);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            _doctorCoordinator.UpdatePatient(_patient);
            healthConditionListBox.Items.Refresh();
        }

        private void deleteHealthCondition(HealthConditionType conditionType)
        {
            Action<string> medicalRecordOperation = conditionType == HealthConditionType.Allergy ? _patient.MedicalRecord.DeleteAllergy : _patient.MedicalRecord.DeleteMedicalCondition;
            var healthConditionListBox = conditionType == HealthConditionType.Allergy ? AllergiesListBox : MedicalHistoryListBox;

            string? selectedCondition = (string)healthConditionListBox.SelectedItem;
            if (selectedCondition == null)
            {
                MessageBox.Show($"You must select {conditionType} in order to delete it");
                return;
            }
            try
            {
                medicalRecordOperation(selectedCondition);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            _doctorCoordinator.UpdatePatient(_patient);
            healthConditionListBox.Items.Refresh();
        }

        private void changePhysicalCharacteristic(bool isHeight)
        {
            Action<int> medicalRecordOperation = isHeight ? _patient.MedicalRecord.ChangeHeight : _patient.MedicalRecord.ChangeWeight;
            var textBox = isHeight ? HeightTextBox : WeightTextBox;

            int newSize = Int32.Parse(textBox.Text);
            try
            {
                medicalRecordOperation(newSize);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            _doctorCoordinator.UpdatePatient(_patient);
            MessageBox.Show("Succeed");
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
