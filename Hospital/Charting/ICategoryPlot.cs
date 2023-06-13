using System.Collections.Generic;

namespace Hospital.Charting
{
    public interface ICategoryPlot
    {
        public void Plot(Dictionary<string, double> categoricalData);
    }
}
