using System;

namespace Hospital.Models.Doctor;

public class Doctor : Person
{
    public Doctor(string firstName, string lastName, string jmbg, string username, string password,
        string specialization) : base(firstName, lastName, jmbg, username, password)
    {
        Specialization = specialization;
        AverageRating = GenerateRandomRating();
    }

    public Doctor()
    {
        Specialization = "Unknown";
        AverageRating = GenerateRandomRating();
    }

    public string Specialization { get; set; }
    public double AverageRating { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType()) return false;

        var other = obj as Doctor;
        if (other == null) return false;
        return Id == other.Id;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public Doctor DeepCopy()
    {
        var copy = new Doctor(FirstName, LastName, Jmbg, Profile.Username, Profile.Password, Specialization)
        {
            Id = Id,
            AverageRating = AverageRating
        };

        return copy;
    }

    private double GenerateRandomRating()
    {
        Random random = new Random();
        return random.NextDouble() * 10; // Generates a random rating between 0 and 10
    }
}