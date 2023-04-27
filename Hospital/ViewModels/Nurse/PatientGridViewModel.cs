using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Models.Patient;
using Hospital.Repositories.Patient;

namespace Hospital.ViewModels.Nurse
{
    public class PatientGridViewModel : ViewModelBase
    {
        private readonly PatientRepository _patientRepository;
        private ObservableCollection<Patient> _patients;
        private Patient? _selectedPatient;

        public PatientGridViewModel()
        {
            _patientRepository = new PatientRepository();
            _patients = new ObservableCollection<Patient>(_patientRepository.GetAll());
        }

        public Patient? SelectedPatient
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
    }
}
