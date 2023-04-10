using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Serialization;

namespace Hospital.Repositories.Nurse
{
    using Hospital.Models.Nurse;

    public class NurseRepository
    {
        private const string FilePath = "../../../Data/nurses.csv";

        public List<Nurse> GetAll()
        {
            return Serializer<Nurse>.FromCSV(FilePath);
        }

        public Nurse? Get(string id)
        {
            return GetAll().FirstOrDefault(nurse => nurse.Id == id);
        }

        public void Add(Nurse nurse)
        {
            var allNurses = GetAll();
            allNurses.Add(nurse);
            Serializer<Nurse>.ToCSV(allNurses, FilePath);
        }

        public void Update(Nurse nurse)
        {
            var allNurses= GetAll();

            var indexToUpdate = allNurses.FindIndex(nurseRecord => nurseRecord.Id == nurse.Id);
            if (indexToUpdate == -1)
                throw new KeyNotFoundException($"Nurse with id {nurse.Id} was not found.");
            allNurses[indexToUpdate] = nurse;

            Serializer<Nurse>.ToCSV(allNurses, FilePath);
        }
        public void Delete(Nurse nurse)
        {
            var allNurses = GetAll();

            if (!allNurses.Remove(nurse))
                throw new KeyNotFoundException($"Nurse with id {nurse.Id} was not found.");

            Serializer<Nurse>.ToCSV(allNurses, FilePath);
        }
    }
}
