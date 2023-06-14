using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Repositories.Feedback;
using ScottPlot;

namespace Hospital.Charting
{
    public class RatingFrequencyPiePlotter: IRatingFrequencyPlotter
    {
        private ScottPlot.WpfPlot wpfPlot;

        public RatingFrequencyPiePlotter(WpfPlot wpfPlot)
        {
            this.wpfPlot = wpfPlot;
        }

        public void PlotRatingFrequencies(Dictionary<int, int> frequencies)
        {
            var labels = frequencies.Keys.Select(e => "Rating " + e.ToString()).ToArray();
            var values = frequencies.Values.Select(e => (double)e).ToArray();

            wpfPlot.Plot.Clear();
            var pie = wpfPlot.Plot.AddPie(values);
            pie.SliceLabels = labels;
            pie.ShowLabels = true;
            pie.ShowValues = true;
            wpfPlot.Refresh();
            wpfPlot.Plot.AxisAuto();
        }
    }
}
