using System;

namespace Hospital.PatientFeedback.Models;

public class DoctorFeedback : Feedback
{
    public DoctorFeedback(string id, string doctorId, int overallRating, int recommendationRating, string comment,
        DateTime dateSubmitted, int doctorQualityRating) : base(id, overallRating, recommendationRating, comment,
        dateSubmitted)
    {
        DoctorQualityRating = doctorQualityRating;
        DoctorId = doctorId;
    }

    public DoctorFeedback(string doctorId, int overallRating, int recommendationRating, string comment,
        int doctorQualityRating) : base(overallRating, recommendationRating, comment)
    {
        DoctorQualityRating = doctorQualityRating;
        DoctorId = doctorId;
    }

    public DoctorFeedback() : base(0, 0, "")
    {
        DoctorQualityRating = 0;
        DoctorId = "";
    }

    public string DoctorId { get; set; }
    public int DoctorQualityRating { get; set; }

    public int[] GetAllRatings()
    {
        return new[] { OverallRating, RecommendationRating, DoctorQualityRating };
    }
}