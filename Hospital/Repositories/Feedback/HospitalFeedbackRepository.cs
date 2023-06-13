using Hospital.Models.Feedback;
using Hospital.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.DTOs;

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

        public AverageHospitalFeedbackGradeByAreaDTO GetAverageGrades()
        {
            var allFeedbacks = GetAll();

            return new AverageHospitalFeedbackGradeByAreaDTO(allFeedbacks.Average(e => e.ServiceQualityRating),
                allFeedbacks.Average(e => e.OverallRating), allFeedbacks.Average(e => e.RecommendationRating),
                allFeedbacks.Average(e => e.CleanlinessRating), allFeedbacks.Average(e => e.SatisfactionRating));
        }
    }
}
