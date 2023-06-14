using System;
using System.Collections.Generic;
using System.Linq;
using Hospital.DTOs;
using Hospital.Models.Feedback;
using Hospital.Serialization;

namespace Hospital.Repositories.Feedback;

public class DoctorFeedbackRepository
{
    private const string FilePath = "../../../Data/doctor_feedbacks.csv";
    private static DoctorFeedbackRepository? _instance;

    private DoctorFeedbackRepository()
    {
    }

    public static DoctorFeedbackRepository Instance => _instance ??= new DoctorFeedbackRepository();

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

    private Dictionary<int, int> GetFrequencies(int[] ratings)
    {
        var frequencies = new Dictionary<int, int>();

        foreach (var possibleRating in ratings.Distinct())
            frequencies[possibleRating] = ratings.Count(e => e == possibleRating);

        return frequencies;
    }

    public List<AverageDoctorRatingDTO> GetDoctorsOrderedByAverageRating()
    {
        var doctorsByRating = from feedback in GetAll()
            group feedback by feedback.DoctorId
            into g
            select new AverageDoctorRatingDTO(g.Key, g.SelectMany(e => e.GetAllRatings()).Average());
        return doctorsByRating.OrderByDescending(e => e.AverageRating).ToList();
    }

    public List<AverageDoctorRatingDTO> GetTop3Doctors()
    {
        var doctorsOrderedByAverageRating = GetDoctorsOrderedByAverageRating();
        return doctorsOrderedByAverageRating.Take(Math.Min(3, doctorsOrderedByAverageRating.Count)).ToList();
    }

    public List<AverageDoctorRatingDTO> GetBottom3Doctors()
    {
        var doctorsOrderedByAverageRating = GetDoctorsOrderedByAverageRating();
        return doctorsOrderedByAverageRating.TakeLast(Math.Min(3, doctorsOrderedByAverageRating.Count)).ToList();
    }

    public Dictionary<int, int> GetOverallRatingFrequencies(string doctorId)
    {
        var overallRatings = GetByDoctorId(doctorId).Select(e => e.OverallRating).ToArray();
        return GetFrequencies(overallRatings);
    }

    public Dictionary<int, int> GetRecommendationRatingFrequencies(string doctorId)
    {
        var recommendationRatings = GetByDoctorId(doctorId).Select(e => e.RecommendationRating).ToArray();
        return GetFrequencies(recommendationRatings);
    }

    public Dictionary<int, int> GetDoctorQualityRatingFrequencies(string doctorId)
    {
        var doctorQualityRatings = GetByDoctorId(doctorId).Select(e => e.DoctorQualityRating).ToArray();
        return GetFrequencies(doctorQualityRatings);
    }

    public AverageDoctorRatingByAreaDto GetAverageRatingsByArea(string doctorId)
    {
        var ratings = GetByDoctorId(doctorId);
        return new AverageDoctorRatingByAreaDto(doctorId, ratings.Average(e => e.OverallRating),
            ratings.Average(e => e.RecommendationRating), ratings.Average(e => e.DoctorQualityRating));
    }
}