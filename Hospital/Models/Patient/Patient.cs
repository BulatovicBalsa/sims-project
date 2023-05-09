namespace Hospital.Models.Patient;

public class Patient : Person
{
    public const int MINIMUM_DAYS_TO_CHANGE_OR_DELETE_EXAMINATION = 1;
    public const int MAX_CHANGES_OR_DELETES_LAST_30_DAYS = 4;
    public const int MAX_ALLOWED_EXAMINATIONS_LAST_30_DAYS = 8;

    public Patient(string firstName, string lastName, string jmbg, string username, string password,
        MedicalRecord medicalRecord) : base(firstName, lastName, jmbg, username, password)
    {
        MedicalRecord = medicalRecord;
        IsBlocked = false;
    }

    public Patient()
    {
        MedicalRecord = new MedicalRecord();
    }

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
