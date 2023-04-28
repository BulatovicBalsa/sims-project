using GalaSoft.MvvmLight.Command;
using Hospital.Coordinators;
using Hospital.Models.Examination;
using Hospital.Models.Patient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Hospital.ViewModels
{
    public class PerformExaminationDialogViewModel : ViewModelBase
    {
        private Examination _examinationToPerform { get; set; }
        private Patient _patientOnExamination { get; set; }
        private readonly DoctorService _doctorService = new DoctorService();

        private string _anamnesis;
        public string Anamnesis
        {
            get { return _anamnesis; }
            set { _anamnesis = value; OnPropertyChanged(nameof(Anamnesis)); }
        }

        public string FirstName { get => _patientOnExamination.FirstName; }

        public string LastName { get => _patientOnExamination.LastName; }

        public string Jmbg { get => _patientOnExamination.Jmbg; }

        public ICommand UpdateExaminationCommand { get; set; }
        public PerformExaminationDialogViewModel(Examination examinationToPerform, Patient patientOnExamination)
        {
            _examinationToPerform = examinationToPerform;
            _patientOnExamination = patientOnExamination;
            Anamnesis = _examinationToPerform.Anamnesis;
            UpdateExaminationCommand = new RelayCommand(UpdateExamination);
        }

        private void UpdateExamination()
        {
            _examinationToPerform.Anamnesis = Anamnesis;
            _doctorService.UpdateExamination(_examinationToPerform);
            MessageBox.Show("Succeed");
        }
    }
}
