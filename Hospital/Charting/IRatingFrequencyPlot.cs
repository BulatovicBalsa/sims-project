using System.Collections.Generic;

namespace Hospital.Charting;

public interface IRatingFrequencyPlot
{
    public void Plot(Dictionary<int, int> ratingFrequencies);
}