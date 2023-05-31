using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.DTOs;
using Hospital.Models.Doctor;
using Hospital.Models.Patient;
using Hospital.Repositories.Examination;
using Hospital.Repositories.Patient;

namespace Hospital.Services
{
    public class HospitalTreatmentService
    {
        private readonly ExaminationService _examinationService = new();

        public HospitalTreatmentService() { }

        public List<MedicalVisitDto> GetHospitalizedPatients(Doctor doctor)
        {
            var designatedPatients = _examinationService.GetViewedPatients(doctor);

            return (from patient in designatedPatients where patient.HospitalTreatmentReferrals.Count > 0 select new MedicalVisitDto(patient, patient.HospitalTreatmentReferrals[0])).ToList();
        }
    }
}
