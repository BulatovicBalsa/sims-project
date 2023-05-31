using System.Collections.ObjectModel;
using Hospital.DTOs;
using Hospital.Models.Doctor;
using Hospital.Models.Patient;
using Hospital.Services;

namespace Hospital.ViewModels
{
    public class VisitHospitalTreatmentPatientsViewModel : ViewModelBase
    {
        private readonly HospitalTreatmentService _hospitalTreatmentService= new();
        private Doctor _doctor;
        private ObservableCollection<MedicalVisitDto> _medicalVisits;
        public ObservableCollection<MedicalVisitDto> MedicalVisits
        {
            get => _medicalVisits;
            set
            {
                _medicalVisits = value;
                OnPropertyChanged(nameof(MedicalVisits));
            }
        }

        public VisitHospitalTreatmentPatientsViewModel(Doctor doctor)
        {
            _doctor = doctor;
            _medicalVisits = new ObservableCollection<MedicalVisitDto>(_hospitalTreatmentService.GetHospitalizedPatients(_doctor));
        }
    }
}
