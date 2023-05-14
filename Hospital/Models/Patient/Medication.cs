using System;
using System.Collections.Generic;
using System.Windows.Documents;

namespace Hospital.Models.Patient
{
    public class Medication
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<string> Allergens { get; set; }

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

        public Medication DeepCopy()
        {
            return new Medication(Id, Name, Allergens);
        }
    }
}
