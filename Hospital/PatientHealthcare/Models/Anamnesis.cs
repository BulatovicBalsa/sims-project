using System.Collections.Generic;

namespace Hospital.PatientHealthcare.Models;

public class Anamnesis
{
    public Anamnesis()
    {
        Symptoms = new List<string>();
        Report = "";
    }

    public List<string> Symptoms { get; }
    public string Report { get; set; }

    public void AddSymptom(string symptom)
    {
        Symptoms.Add(symptom);
    }
}