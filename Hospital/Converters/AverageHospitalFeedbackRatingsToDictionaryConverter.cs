﻿using System.Collections.Generic;
using Hospital.DTOs;

namespace Hospital.Converters;

public class AverageHospitalFeedbackRatingsToDictionaryConverter
{
    public Dictionary<string, double> Convert(AverageHospitalFeedbackRatingByAreaDto averageRatingByArea)
    {
        var dictionary = new Dictionary<string, double>
        {
            ["Overall rating"] = averageRatingByArea.OverallRating,
            ["Service quality"] = averageRatingByArea.ServiceQuality,
            ["Cleanliness"] = averageRatingByArea.CleanlinessRating,
            ["Patient satisfaction"] = averageRatingByArea.PatientSatisfactionRating,
            ["Recommendation rating"] = averageRatingByArea.RecommendationRating
        };
        return dictionary;
    }
}