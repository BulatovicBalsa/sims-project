using Hospital.Workers.Models;
using Hospital.Workers.Services;

namespace Hospital.PatientHealthcare.Models;

public class Referral
{
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

    public Referral(string specialization, Doctor? doctor)
    {
        Specialization = specialization;
        Doctor = doctor;
    }

    public string Specialization { get; set; }
    public Doctor? Doctor { get; set; }

    public string ComboBoxString => $"{Doctor?.FirstName} {Doctor?.LastName} {Specialization}";

    public void AssignDoctor()
    {
        var doctorService = new DoctorService();
        var qualifiedDoctor = doctorService.GetQualifiedDoctors(Specialization)[0];

        Doctor = qualifiedDoctor;
    }

    public void DeepCopy(Referral other)
    {
        Doctor = other.Doctor;
        Specialization = other.Specialization;
    }

    public bool IsDefault()
    {
        return Specialization == "";
    }

    public override string ToString()
    {
        return $"{Specialization};{Doctor?.Id}";
    }

    public override bool Equals(object? obj)
    {
        if (obj is not Referral objAsReferral) return false;
        return objAsReferral.Doctor == Doctor && objAsReferral.Specialization == Specialization;
    }
}