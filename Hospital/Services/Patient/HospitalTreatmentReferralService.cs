using System.Collections.Generic;
using System.Linq;
using Hospital.Models.Patient;
using Hospital.Repositories.Patient;

namespace Hospital.Services.Patient;
public class HospitalTreatmentReferralService
{
    private readonly PatientRepository _patientRepository;

    public HospitalTreatmentReferralService()
    {
        _patientRepository = PatientRepository.Instance;
    }

    public List<HospitalTreatmentReferral> GetAllActiveHospitalTreatmentReferrals()
    {
        var allPatients = _patientRepository.GetAll();
        var activeHospitalTreatmentReferrals =
            allPatients.Select(patient => patient.GetActiveHospitalTreatmentReferral());

        return activeHospitalTreatmentReferrals.Where(referral => referral != null).ToList()!;
    }
}
