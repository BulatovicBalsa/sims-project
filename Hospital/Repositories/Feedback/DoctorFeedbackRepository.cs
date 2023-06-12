


using Hospital.Models.Feedback;
using Hospital.Serialization;
using System.Collections.Generic;
using System.Linq;

namespace Hospital.Repositories.Feedback
{
    public class DoctorFeedbackRepository
    {
        private const string FilePath = "../../../Data/doctor_feedbacks.csv";
        private static DoctorFeedbackRepository? _instance;

        public static DoctorFeedbackRepository Instance => _instance ??= new DoctorFeedbackRepository();

        private DoctorFeedbackRepository() { }

        public List<DoctorFeedback> GetAll()
        {
            return Serializer<DoctorFeedback>.FromCSV(FilePath);
        }

        public void Add(DoctorFeedback feedback)
        {
            var allFeedbacks = GetAll();

            allFeedbacks.Add(feedback);

            Serializer<DoctorFeedback>.ToCSV(allFeedbacks, FilePath);
        }

        public List<DoctorFeedback> GetByDoctorId(string doctorId)
        {
            return GetAll().Where(feedback => feedback.DoctorId == doctorId).ToList();
        }
    }
}