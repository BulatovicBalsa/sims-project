using System;

namespace Hospital.Core.PatientFeedback.Models;

public abstract class Feedback
{
    public Feedback(string id, int overallRating, int recommendationRating, string comment, DateTime dateSubmitted)
    {
        Id = id;
        OverallRating = overallRating;
        RecommendationRating = recommendationRating;
        Comment = comment;
        DateSubmitted = dateSubmitted;
    }

    public Feedback(int overallRating, int recommendationRating, string comment)
    {
        Id = Guid.NewGuid().ToString();
        OverallRating = overallRating;
        RecommendationRating = recommendationRating;
        Comment = comment;
        DateSubmitted = DateTime.Now;
    }

    public Feedback()
    {
        Id = Guid.NewGuid().ToString();
        OverallRating = 0;
        RecommendationRating = 0;
        Comment = "";
        DateSubmitted = DateTime.Now;
    }

    public string Id { get; set; }
    public int OverallRating { get; set; }
    public int RecommendationRating { get; set; }
    public string Comment { get; set; }
    public DateTime DateSubmitted { get; set; }
}