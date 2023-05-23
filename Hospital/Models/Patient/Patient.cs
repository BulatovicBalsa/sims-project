using System.Collections.Generic;

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
    }

    public Patient()
    {
        MedicalRecord = new MedicalRecord();
        Referrals = new List<Referral>();
    }

    public List<Referral> Referrals { get; set; }
    public bool IsBlocked { get; set; }
    public MedicalRecord MedicalRecord { get; set; }

    public Patient DeepCopy()
    {
        var copy = new Patient(FirstName, LastName, Jmbg, Profile.Username, Profile.Password, MedicalRecord.DeepCopy())
        {
            Id = Id,
            IsBlocked = IsBlocked
        };

        return copy;
    }
}
