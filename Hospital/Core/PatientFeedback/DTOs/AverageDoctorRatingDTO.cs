namespace Hospital.Core.PatientFeedback.DTOs;

public class AverageDoctorRatingDto
{
    public AverageDoctorRatingDto(string doctorId, double averageRating)
    {
        DoctorId = doctorId;
        AverageRating = averageRating;
    }

    public string DoctorId { get; }
    public double AverageRating { get; }
}