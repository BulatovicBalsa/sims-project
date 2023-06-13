using System.Collections.Generic;
using System.Linq;
using Hospital.DTOs;
using Hospital.Models.Feedback;
using Hospital.Serialization;

namespace Hospital.Repositories.Feedback;

public class HospitalFeedbackRepository
{
    private const string FilePath = "../../../Data/hospital_feedbacks.csv";
    private static HospitalFeedbackRepository? _instance;

    private HospitalFeedbackRepository()
    {
    }

    public static HospitalFeedbackRepository Instance => _instance ??= new HospitalFeedbackRepository();

    public List<HospitalFeedback> GetAll()
    {
        return Serializer<HospitalFeedback>.FromCSV(FilePath);
    }

    public void Add(HospitalFeedback feedback)
    {
        var allFeedbacks = GetAll();

        allFeedbacks.Add(feedback);

        Serializer<HospitalFeedback>.ToCSV(allFeedbacks, FilePath);
    }

    public AverageHospitalFeedbackRatingByAreaDTO GetAverageRatings()
    {
        var allFeedbacks = GetAll();

        return new AverageHospitalFeedbackRatingByAreaDTO(allFeedbacks.Average(e => e.ServiceQualityRating),
            allFeedbacks.Average(e => e.OverallRating), allFeedbacks.Average(e => e.RecommendationRating),
            allFeedbacks.Average(e => e.CleanlinessRating), allFeedbacks.Average(e => e.SatisfactionRating));
    }

    private Dictionary<int, int> GetFrequencies(int[] ratings)
    {
        var frequencies = new Dictionary<int, int>();

        foreach (var possibleRating in ratings.Distinct())
            frequencies[possibleRating] = ratings.Count(e => e == possibleRating);

        return frequencies;
    }

    public Dictionary<int, int> GetServiceQualityRatingFrequencies()
    {
        var serviceRatings = GetAll().Select(e => e.ServiceQualityRating).ToArray();
        return GetFrequencies(serviceRatings);
    }

    public Dictionary<int, int> GetOverallRatingFrequencies()
    {
        var overallRatings = GetAll().Select(e => e.OverallRating).ToArray();
        return GetFrequencies(overallRatings);
    }

    public Dictionary<int, int> GetCleanlinessRatingFrequencies()
    {
        var cleanlinessRatings = GetAll().Select(e => e.CleanlinessRating).ToArray();
        return GetFrequencies(cleanlinessRatings);
    }

    public Dictionary<int, int> GetPatientSatisfactionRatingFrequencies()
    {
        var satisfactionRatings = GetAll().Select(e => e.SatisfactionRating).ToArray();
        return GetFrequencies(satisfactionRatings);
    }

    public Dictionary<int, int> GetRecommendationRatingFrequencies()
    {
        var recommendationRatings = GetAll().Select(e => e.RecommendationRating).ToArray();
        return GetFrequencies(recommendationRatings);
    }
}