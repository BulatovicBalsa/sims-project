using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Models.Patient
{
    public class Anamnesis
    {
        public List<string> Symptoms { get; }
        public string Report { get; set; }

        public Anamnesis()
        {
            Symptoms = new List<string>();
        }

        public void AddSymptom(string symptom)
        {
            Symptoms.Add(symptom);
        }
    }
}