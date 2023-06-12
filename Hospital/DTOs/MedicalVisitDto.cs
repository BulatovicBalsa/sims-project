using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Models.Patient;

namespace Hospital.DTOs
{
    public class MedicalVisitDto
    {
        public Patient Patient { get; set; }
        public HospitalTreatmentReferral Referral { get; set; }

        public MedicalVisitDto(Patient patient, HospitalTreatmentReferral referral)
        {
            Patient = patient;
            Referral = referral;
        }
    }
}
