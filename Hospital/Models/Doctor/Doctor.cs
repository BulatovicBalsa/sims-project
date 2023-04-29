﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Models.Doctor
{
    public class Doctor : Person
    {
        private const int DAYS_TO_CHECK = 3;

        public string Specialization { get; set; }
        public Doctor(string firstName, string lastName, string jmbg, string username, string password,string specialization) : base(firstName, lastName, jmbg, username, password) 
        {
            Specialization = specialization;
        }
        public Doctor() : base() { }

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Doctor? other = obj as Doctor;
            if (other == null) { return false; }
            return Id == other.Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public Doctor DeepCopy()
        {
            Doctor copy = new Doctor(FirstName, LastName, Jmbg, Profile.Username, Profile.Password,Specialization)
            {
                Id = this.Id
            };

            return copy;
        }
    }
}
