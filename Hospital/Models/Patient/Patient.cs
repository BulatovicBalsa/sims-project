using System.Collections.Generic;
using System.Linq;

namespace Hospital.Models.Patient;

public class Patient : Person
{
    public const int MinimumDaysToChangeOrDeleteExamination = 1;
    public const int MaxChangesOrDeletesLast30Days = 4;
    public const int MaxAllowedExaminationsLast30Days = 8;

    public Patient(string firstName, string lastName, string jmbg, string username, string password,
        MedicalRecord medicalRecord) : base(firstName, lastName, jmbg, username, password)
    {
        MedicalRecord = medicalRecord;
        IsBlocked = false;
        Referrals = new List<Referral>();
        HospitalTreatmentReferrals = new List<HospitalTreatmentReferral>();
    }

    public Patient()
    {
        MedicalRecord = new MedicalRecord();
        Referrals = new List<Referral>();
        HospitalTreatmentReferrals = new List<HospitalTreatmentReferral>();
    }

    public List<Referral> Referrals { get; set; }
    public bool IsBlocked { get; set; }
    public MedicalRecord MedicalRecord { get; set; }
    public List<HospitalTreatmentReferral> HospitalTreatmentReferrals { get; set; }
    public Patient DeepCopy()
    {
        var copy = new Patient(FirstName, LastName, Jmbg, Profile.Username, Profile.Password, MedicalRecord.DeepCopy())
        {
            Id = Id,
            IsBlocked = IsBlocked
        };

        return copy;
    }

    public bool IsAllergicTo(Medication medication)
    {
        return medication.Allergens.Any(allergen => MedicalRecord.Allergies.Conditions.Contains(allergen));
    }
}
