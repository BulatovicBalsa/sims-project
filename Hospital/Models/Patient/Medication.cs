using System;
using System.Collections.Generic;
using System.Windows.Documents;

namespace Hospital.Models.Patient;

public class Medication
{
    public Medication()
    {
        Id = Guid.NewGuid().ToString();
        Allergens = new List<string>();
        Name = "";
    }

    public Medication(string id, string name, List<string> allergens)
    {
        Id = id;
        Name = name;
        Allergens = allergens;
    }

    public Medication(string name, List<string> allergens)
    {
        Id = Guid.NewGuid().ToString();
        Name = name;
        Allergens = allergens;
    }

    public string Id { get; set; }
    public string Name { get; set; }
    public List<string> Allergens { get; set; }

    public Medication DeepCopy()
    {
        return new Medication(Id, Name, Allergens);
    }
}