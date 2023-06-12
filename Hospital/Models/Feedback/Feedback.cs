using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Models.Feedback
{
    public abstract class Feedback
    {
        public string Id { get; set; }
        public int OverallRating { get; set; }
        public int RecommendationRating { get; set; }
        public string Comment { get; set; }
        public DateTime DateSubmitted { get; set; }

        public Feedback(string id,int overallRating, int recommendationRating, string comment, DateTime dateSubmitted)
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


    }
}
