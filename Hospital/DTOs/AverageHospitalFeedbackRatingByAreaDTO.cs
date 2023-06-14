namespace Hospital.DTOs;

public class AverageHospitalFeedbackRatingByAreaDto
{
    public AverageHospitalFeedbackRatingByAreaDto(double serviceQuality, double overallRating,
        double recommendationRating, double cleanlinessRating, double patientSatisfactionRating)
    {
        ServiceQuality = serviceQuality;
        OverallRating = overallRating;
        RecommendationRating = recommendationRating;
        CleanlinessRating = cleanlinessRating;
        PatientSatisfactionRating = patientSatisfactionRating;
    }

    public double ServiceQuality { get; }
    public double OverallRating { get; }
    public double RecommendationRating { get; }
    public double CleanlinessRating { get; }
    public double PatientSatisfactionRating { get; }
}