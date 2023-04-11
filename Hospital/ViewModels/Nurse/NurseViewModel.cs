using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Hospital.Models.Patient;
using Hospital.Repositories.Patient;

namespace Hospital.ViewModels.Nurse
{
    public class NurseViewModel : ViewModelBase
    {
        private ObservableCollection<Patient> _patients;
        private PatientRepository _patientRepository;
        private Patient _selectedPatient;


        public Patient SelectedPatient
        {
            get => _selectedPatient;
            set
            {
                _selectedPatient = value;
                OnPropertyChanged(nameof(SelectedPatient));
            }
        }
        public ObservableCollection<Patient> Patients
        {
            get => _patients;
            set
            {
                _patients = value;
                OnPropertyChanged(nameof(Patients));
            }
        }

        public NurseViewModel()
        {
            _patientRepository = new PatientRepository();
            _patients = new ObservableCollection<Patient>(_patientRepository.GetAll());
            SelectedPatient = null;

            _patients.CollectionChanged += (sender, e) =>
            {
                if (e.Action == NotifyCollectionChangedAction.Add)
                {
                    if (e.NewItems != null)
                    {
                        foreach (var item in e.NewItems)
                        {
                            _patientRepository.Add((Patient) item);
                        }
                    }
                }
            };

            DeletePatientCommand = new ViewModelCommand(ExecuteDeletePatientCommand, CanExecuteDeletePatientCommand);
            UpdatePatientCommand = new ViewModelCommand(ExecuteUpdatePatientCommand, CanExecuteUpdatePatientCommand);
        }

        public ICommand DeletePatientCommand { get; }
        public ICommand UpdatePatientCommand { get; }

        private bool CanExecuteDeletePatientCommand(object obj)
        {
            return SelectedPatient != null;
        }

        private void ExecuteDeletePatientCommand(object obj)
        {
            _patientRepository.Delete(SelectedPatient);
            _patients.Remove(SelectedPatient);
        }

        private bool CanExecuteUpdatePatientCommand(object obj)
        {
            return SelectedPatient != null;
        }

        private void ExecuteUpdatePatientCommand(object obj)
        {
            _patientRepository.Update(SelectedPatient);
        }


    }
}
