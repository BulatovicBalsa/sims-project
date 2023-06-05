using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Models.Feedback
{
    public abstract class Feedback
    {
        public int Id { get; set; }
        public int Rating { get; set; }
        public int RecommendationRating { get; set; }
        public string Comment { get; set; }
        public DateTime DateSubmitted { get; set; }

        public Feedback(int rating, int recommendationRating, string comment, DateTime dateSubmitted)
        {
            Rating = rating;
            RecommendationRating = recommendationRating;
            Comment = comment;
            DateSubmitted = dateSubmitted;
        }
        public Feedback(int rating, int recommendationRating, string comment)
        {
            Rating = rating;
            RecommendationRating = recommendationRating;
            Comment = comment;
            DateSubmitted = DateTime.Now;
        }


    }
}
