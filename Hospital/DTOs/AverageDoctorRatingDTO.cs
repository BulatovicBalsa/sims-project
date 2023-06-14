namespace Hospital.DTOs;

public class AverageDoctorRatingDTO
{
    public AverageDoctorRatingDTO(string doctorId, double averageRating)
    {
        DoctorId = doctorId;
        AverageRating = averageRating;
    }

    public string DoctorId { get; }
    public double AverageRating { get; }
}