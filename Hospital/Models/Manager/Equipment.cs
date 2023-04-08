using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Models.Manager
{
    public class Equipment
    {
        public int Id { get; set; }

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

        public Equipment(int id, string name, EquipmentType type)
        {
            Id = id;
            Name = name; 
            Type = type;
        }
    }
}
