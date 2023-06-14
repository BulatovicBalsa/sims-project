using System.Collections.Generic;
using Hospital.DTOs;

namespace Hospital.Converters;

public class AverageDoctorRatingsByAreaToDictionaryConverter
{
    public Dictionary<string, double> Convert(AverageDoctorRatingByAreaDto averageDoctorRatingByAreaDto)
    {
        return new Dictionary<string, double>
        {
            ["Overall rating"] = averageDoctorRatingByAreaDto.OverallRating,
            ["Quality rating"] = averageDoctorRatingByAreaDto.DoctorQualityRating,
            ["Recommendation rating"] = averageDoctorRatingByAreaDto.RecommendationRating
        };
    }
}