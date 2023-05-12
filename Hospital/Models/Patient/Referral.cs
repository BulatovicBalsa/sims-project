namespace Hospital.Models.Patient;
using Doctor;
public class Referral
{
    public string Specialization { get; set; }
    public Doctor? Doctor { get; set; }

    public Referral()
    {
        Specialization = "";
    }

    public Referral(Doctor doctor)
    {
        Doctor = doctor;
        Specialization = doctor.Specialization;
    }

    public Referral(string specialization)
    {
        Specialization = specialization;
    }
}