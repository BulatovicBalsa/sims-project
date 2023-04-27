﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Models.Patient
{
    public class MedicalRecord
    {
        public int Weight { get; set; }
        public int Height { get; set; }
        public List<string> Allergies { get; set; }
        public List<string> MedicalHistory { get; set; }
        //public List<Prescription> Prescriptions { get; set; }

        public MedicalRecord()
        {
            Weight = 0;
            Height = 0;
            Allergies = new List<string>();
            MedicalHistory = new List<string>();
            //Prescriptions = new List<Prescription>();
        }

        public MedicalRecord(int height, int weight)
        {
            Height = height;
            Weight = weight;
            Allergies = new List<string>();
            MedicalHistory = new List<string>();
            //Prescriptions = new List<Prescription>();
        }

        public MedicalRecord(int height, int weight, List<string> allergies, List<string> medicalHistory)
        {
            Height = height;
            Weight = weight;
            Allergies = allergies;
            MedicalHistory = medicalHistory;
            //Prescriptions = prescriptions;
        }

        public MedicalRecord DeepCopy()
        {
            MedicalRecord copy = new MedicalRecord(Height, Weight)
            {
                Allergies = new List<string>(Allergies),
                MedicalHistory = new List<string>(MedicalHistory),
                //Prescriptions = new List<Prescription>(Prescriptions.Select(p => p.DeepCopy()))
            };

            return copy;
        }
    }
}
