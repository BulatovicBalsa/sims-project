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

        public DoctorFeedback(string doctorId,int rating, int recommendationRating, string comment, DateTime dateSubmitted, int doctorQualityRating) : base(rating, recommendationRating, comment, dateSubmitted)
        {
            DoctorQualityRating = doctorQualityRating;
            DoctorId = doctorId;
        }
        public DoctorFeedback(string doctorId,int rating, int recommendationRating, string comment, int doctorQualityRating) : base(rating, recommendationRating, comment)
        {
            DoctorQualityRating = doctorQualityRating;
            DoctorId = doctorId;
        }
    }
}
