using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Models.Patient;
using Hospital.Repositories.Patient;

namespace Hospital.ViewModels.Nurse
{
    public class NurseViewModel : ViewModelBase
    {
        private List<Patient> _patients;

        public List<Patient> Patients
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
            _patients = new PatientRepository().GetAll();
        }
    }
}
