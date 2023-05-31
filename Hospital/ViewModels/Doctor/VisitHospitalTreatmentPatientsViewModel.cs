using System.Collections.ObjectModel;
using Hospital.Models.Doctor;
using Hospital.Models.Patient;
using Hospital.Services;

namespace Hospital.ViewModels
{
    public class VisitHospitalTreatmentPatientsViewModel : ViewModelBase
    {
        private readonly PatientService _patientService = new();
        private Doctor _doctor;
        private ObservableCollection<HospitalTreatmentReferral> _patientsReferrals;
        public ObservableCollection<HospitalTreatmentReferral> PatientsReferrals
        {
            get => _patientsReferrals;
            set
            {
                _patientsReferrals = value;
                OnPropertyChanged(nameof(PatientsReferrals));
            }
        }

        public VisitHospitalTreatmentPatientsViewModel(Doctor doctor)
        {
            _doctor = doctor;
            _patientsReferrals = new ObservableCollection<HospitalTreatmentReferral>(_patientService.GetHospitalizedPatients(_doctor));
        }
    }
}
