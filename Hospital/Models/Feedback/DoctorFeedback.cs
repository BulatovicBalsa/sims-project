using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Models;
using DoctorModel = Hospital.Models.Doctor.Doctor;
namespace Hospital.Models.Feedback
{
    
    public class DoctorFeedback : Feedback
    {   
        public DoctorModel Doctor { get; set; }
        public int DoctorQualityRating { get; set; }

        public DoctorFeedback(DoctorModel doctor,int rating, int recommendationRating, string comment, DateTime dateSubmitted, int doctorQualityRating) : base(rating, recommendationRating, comment, dateSubmitted)
        {
            DoctorQualityRating = doctorQualityRating;
            Doctor = doctor;
        }
        public DoctorFeedback(DoctorModel doctor,int rating, int recommendationRating, string comment, int doctorQualityRating) : base(rating, recommendationRating, comment)
        {
            DoctorQualityRating = doctorQualityRating;
            Doctor = doctor;
        }
    }
}
