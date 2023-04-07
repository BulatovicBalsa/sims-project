using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Models.Manager
{
    public class Equipment
    {
        private string _name;


        public string Name
        {
            get => _name;
            set => _name = value;
        }
        
        private EquipmentType _type;
        
        public EquipmentType Type
        {
            get => _type;
            set => _type = value;
        }

        public enum EquipmentType
        {
           FURNITURE, HALLWAY_EQUIPMENT, EXAMINATION_EQUIPMENT, OPERATION_EQUIPMENT 
        }

        public Equipment()
        {
            _name = "";
        }

        public Equipment(string name, EquipmentType type)
        {
            Name = name; 
            Type = type;
        }
    }
}
