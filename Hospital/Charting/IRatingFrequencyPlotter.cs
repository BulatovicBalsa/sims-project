using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Charting
{
    public interface IRatingFrequencyPlotter
    {
        public void PlotRatingFrequencies(Dictionary<int, int> ratingFrequencies);
    }
}
