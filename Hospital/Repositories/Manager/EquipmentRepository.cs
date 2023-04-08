using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Models.Manager;
using CsvHelper;
using Hospital.Serialization;

namespace Hospital.Repositories.Manager
{


    public class EquipmentRepository
    {
        private const String FilePath = "../../../Data/equipment.csv";

        public List<Equipment> GetAll()
        {
            return Serializer<Equipment>.FromCSV(FilePath);
        }

        public Equipment? GetById(string id)
        {
            return GetAll().Find(equipment => equipment.Id == id);
        }

        public void Add(Equipment equipment)
        {
            var allEquipment = GetAll();

            allEquipment.Add(equipment);
            
            Serializer<Equipment>.ToCSV(allEquipment, FilePath);
        }
        
        public void Update(Equipment equipment)
        {
            var allEquipment = GetAll();

            var indexToUpdate = allEquipment.FindIndex(e => e.Id == equipment.Id);            
            if (indexToUpdate == -1)
            {
                throw new KeyNotFoundException();
            }

            allEquipment[indexToUpdate] = equipment;

            Serializer<Equipment>.ToCSV(allEquipment, FilePath);
        }

        public void Delete(Equipment equipment)
        {
            var allEquipment = GetAll();

            var indexToDelete = allEquipment.FindIndex(e => e.Id == equipment.Id);
            if (indexToDelete == -1)
            {
                throw new KeyNotFoundException();
            }

            allEquipment.RemoveAt(indexToDelete);

            Serializer<Equipment>.ToCSV(allEquipment, FilePath);
        }
    }
}
