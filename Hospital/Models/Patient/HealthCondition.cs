﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Models.Patient
{
    public class HealthCondition
    {
        public  HealthConditionType Type { get; private set; }
        public List<string> Conditions { get; set; }

        public HealthCondition(HealthConditionType type)
        {
            Type = type;
            Conditions = new List<string>();
        }

        public HealthCondition(HealthConditionType type, List<string> conditions)
        {
            Type = type;
            Conditions = conditions;
        }

        public void Add(string conditionToAdd, HealthConditionType conditionType)
        {
            conditionToAdd = conditionToAdd.Trim();

            if (string.IsNullOrEmpty(conditionToAdd)) throw new ArgumentException($"{conditionType} name can't be empty");
            if (Conditions.Contains(conditionToAdd))
                throw new ArgumentException($"{conditionToAdd} already exists in medical record");
            Conditions.Add(conditionToAdd);
        }

        public void Delete(string selectedCondition, HealthConditionType conditionType)
        {
            if (!Conditions.Contains(selectedCondition))
                throw new ArgumentException($"{selectedCondition} doesn't exist in this patient's medical record");
            Conditions.Remove(selectedCondition);
        }

        public void Update(string selectedCondition, string updatedCondition, HealthConditionType conditionType)
        {
            updatedCondition = updatedCondition.Trim();
            var indexToUpdate = Conditions.IndexOf(selectedCondition);
            if (indexToUpdate == -1)
                throw new ArgumentException($"{selectedCondition} doesn't exist in this patient's medical record");
            if (string.IsNullOrEmpty(updatedCondition)) throw new ArgumentException($"{conditionType} name can't be empty");
            if (Conditions.Contains(updatedCondition))
                throw new ArgumentException($"{updatedCondition} already exist in this patient's medical record");
            Conditions[indexToUpdate] = updatedCondition;
        }
    }
}
