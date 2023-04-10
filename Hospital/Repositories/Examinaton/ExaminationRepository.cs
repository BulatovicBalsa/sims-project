using Hospital.Models.Examination;
using Hospital.Models.Manager;
using Hospital.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Repositories.Examinaton
{
    public class ExaminationRepository
    {
        private const string FilePath = "../../../Data/examination.csv";

        public List<Examination> GetAll()
        {
            return Serializer<Examination>.FromCSV(FilePath);
        }

        public Examination? GetById(string id)
        {
            return GetAll().Find(examination => examination.Id == id);
        }

        public void Add(Examination examination)
        {
            var allExamination = GetAll();

            allExamination.Add(examination);

            Serializer<Examination>.ToCSV(allExamination, FilePath);
        }

        public void Update(Examination examination)
        {
            var allExamination = GetAll();

            var indexToUpdate = allExamination.FindIndex(e => e.Id == examination.Id);
            if (indexToUpdate == -1) throw new KeyNotFoundException();

            allExamination[indexToUpdate] = examination;

            Serializer<Examination>.ToCSV(allExamination, FilePath);
        }

        public void Delete(Examination examination)
        {
            var allExamination = GetAll();

            var indexToDelete = allExamination.FindIndex(e => e.Id == examination.Id);
            if (indexToDelete == -1) throw new KeyNotFoundException();

            allExamination.RemoveAt(indexToDelete);

            Serializer<Examination>.ToCSV(allExamination, FilePath);
        }
    }
}
