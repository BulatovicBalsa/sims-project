﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Models.Manager
{
    public class Equipment
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public EquipmentType Type { get; set; }


        public enum EquipmentType
        {
           FURNITURE, HALLWAY_EQUIPMENT, EXAMINATION_EQUIPMENT, OPERATION_EQUIPMENT 
        }

        public Equipment()
        {
            Name = "";
        }

        public Equipment(string id, string name, EquipmentType type)
        {
            Id = id;
            Name = name; 
            Type = type;
        }


        public Equipment(string name, EquipmentType type)
        {
            Id = System.Guid.NewGuid().ToString();
            Name = name; 
            Type = type;
        }
    }
}
