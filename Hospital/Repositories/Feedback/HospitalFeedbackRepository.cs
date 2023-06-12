using Hospital.Models.Feedback;
using Hospital.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Repositories.Feedback
{
    public class HospitalFeedbackRepository
    {
        private const string FilePath = "../../../Data/hospital_feedbacks.csv";
        private static HospitalFeedbackRepository? _instance;

        public static HospitalFeedbackRepository Instance => _instance ??= new HospitalFeedbackRepository();

        private HospitalFeedbackRepository() { }

        public List<HospitalFeedback> GetAll()
        {
            return Serializer<HospitalFeedback>.FromCSV(FilePath);
        }

        public void Add(HospitalFeedback feedback)
        {
            var allFeedbacks = GetAll();

            allFeedbacks.Add(feedback);

            Serializer<HospitalFeedback>.ToCSV(allFeedbacks, FilePath);
        }
    }
}
