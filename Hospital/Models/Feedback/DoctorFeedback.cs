using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Models;

namespace Hospital.Models.Feedback
{
    
    public class DoctorFeedback : Feedback
    {   
        public string DoctorId { get; set; }
        public int DoctorQualityRating { get; set; }

        public DoctorFeedback(string id,string doctorId,int overallRating, int recommendationRating, string comment, DateTime dateSubmitted, int doctorQualityRating) : base(id,overallRating, recommendationRating, comment, dateSubmitted)
        {
            DoctorQualityRating = doctorQualityRating;
            DoctorId = doctorId;
        }
        public DoctorFeedback(string doctorId,int overallRating, int recommendationRating, string comment, int doctorQualityRating) : base(overallRating, recommendationRating, comment)
        {
            DoctorQualityRating = doctorQualityRating;
            DoctorId = doctorId;
        }
        public DoctorFeedback() : base(0, 0, "")
        {
            DoctorQualityRating = 0;
            DoctorId = "";
        }

        public int[] GetAllRatings()
        {
            return new int[] { OverallRating, RecommendationRating, DoctorQualityRating};

        }
    }
}
