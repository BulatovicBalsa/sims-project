using System.Collections.Generic;

namespace Hospital.GUI.Charting;

public interface IRatingFrequencyPlot
{
    public void Plot(Dictionary<int, int> ratingFrequencies);
}