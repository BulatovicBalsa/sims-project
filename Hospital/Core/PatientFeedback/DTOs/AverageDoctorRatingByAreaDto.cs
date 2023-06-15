namespace Hospital.Core.PatientFeedback.DTOs;

public class AverageDoctorRatingByAreaDto
{
    public AverageDoctorRatingByAreaDto(string doctorId, double overallRating, double recommendationRating,
        double doctorQualityRating)
    {
        DoctorId = doctorId;
        OverallRating = overallRating;
        RecommendationRating = recommendationRating;
        DoctorQualityRating = doctorQualityRating;
    }

    public string DoctorId { get; }
    public double OverallRating { get; }
    public double RecommendationRating { get; }
    public double DoctorQualityRating { get; }
}