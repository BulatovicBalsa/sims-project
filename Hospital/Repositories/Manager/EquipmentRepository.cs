using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Models.Manager;
using CsvHelper;

namespace Hospital.Repositories.Manager
{


    public class EquipmentRepository
    {
        private const String FilePath = "../../../Data/equipment.csv";

        public List<Equipment> GetAll()
        {
            using StreamReader reader = new StreamReader(FilePath);
            using var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture);
            return csvReader.GetRecords<Equipment>().ToList();
        }

        public Equipment? GetById(int id)
        {
            return GetAll().Find(equipment => equipment.Id == id);
        }

        public void Add(Equipment equipment)
        {
            var allEquipment = GetAll();

            allEquipment.Add(equipment);
            
            using StreamWriter writer = new StreamWriter(FilePath);
            using CsvWriter csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture);
            csvWriter.WriteRecords(allEquipment);
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

            using StreamWriter writer = new StreamWriter(FilePath);
            using CsvWriter csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture);
            csvWriter.WriteRecords(allEquipment);
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

            //TODO: Extract saving to file
            using StreamWriter writer = new StreamWriter(FilePath);
            using CsvWriter csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture);
            csvWriter.WriteRecords(allEquipment);
        }
    }
}
