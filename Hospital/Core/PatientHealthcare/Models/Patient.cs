using System;
using System.Collections.Generic;
using System.Linq;
using Hospital.Core.Accounts.Models;
using Hospital.Core.Pharmacy.Models;

namespace Hospital.Core.PatientHealthcare.Models;

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
        NotificationTime = 30;
        Referrals = new List<Referral>();
        HospitalTreatmentReferrals = new List<HospitalTreatmentReferral>();
    }

    public Patient()
    {
        MedicalRecord = new MedicalRecord();
        Referrals = new List<Referral>();
        HospitalTreatmentReferrals = new List<HospitalTreatmentReferral>();
    }

    public int NotificationTime { get; set; }

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

    public void RemoveReferral(Referral referral)
    {
        Referrals.Remove(referral);
    }

    public bool IsHospitalized()
    {
        return HospitalTreatmentReferrals.Any(referral => referral.IsActive());
    }

    public HospitalTreatmentReferral? GetActiveHospitalTreatmentReferral()
    {
        return HospitalTreatmentReferrals.FirstOrDefault(referral => referral!.IsActive(), null);
    }

    public void ReleaseHospitalTreatmentReferral(HospitalTreatmentReferral selectedVisitReferral)
    {
        var referralToRelease =
            HospitalTreatmentReferrals.FirstOrDefault(referral => referral.Equals(selectedVisitReferral));
        referralToRelease!.Release = DateTime.Today;
    }

    public bool HasUnusedHospitalTreatmentReferral()
    {
        return HospitalTreatmentReferrals.Any(referral => referral.Admission == null && referral.Release == null);
    }

    public HospitalTreatmentReferral GetFirstUnusedHospitalTreatmentReferral()
    {
        return HospitalTreatmentReferrals.First(referral => referral.Admission == null && referral.Release == null);
    }

    public void UpdateHospitalTreatmentReferral(HospitalTreatmentReferral referral)
    {
        var indexToUpdate = HospitalTreatmentReferrals.IndexOf(referral);

        if (indexToUpdate == -1)
            throw new KeyNotFoundException();

        HospitalTreatmentReferrals[indexToUpdate] = referral;
    }
}