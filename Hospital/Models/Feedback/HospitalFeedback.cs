using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Models.Feedback
{
    public class HospitalFeedback : Feedback
    {
        public int ServiceQualityRating { get; set; }
        public int CleanlinessRating { get; set; }
        public int SatisfactionRating { get; set; }
        
        public HospitalFeedback(int rating, int recommendationRating, string comment, DateTime dateSubmitted, int serviceQualityRating, int cleanlinessRating, int patientSatisfactionRating) : base(rating, recommendationRating, comment, dateSubmitted)
        {
            ServiceQualityRating = serviceQualityRating;
            CleanlinessRating = cleanlinessRating;
            SatisfactionRating = patientSatisfactionRating;
        }
        public HospitalFeedback(int rating, int recommendationRating, string comment, int serviceQualityRating, int cleanlinessRating, int patientSatisfactionRating) : base(rating, recommendationRating, comment)
        {
            ServiceQualityRating = serviceQualityRating;
            CleanlinessRating = cleanlinessRating;
            SatisfactionRating = patientSatisfactionRating;
        }
    }
}
