using System.Collections.Generic;
using System.Linq;
using ScottPlot;
using ScottPlot.Plottable;

namespace Hospital.Charting;

public class RatingFrequencyPiePlot : IRatingFrequencyPlot
{
    private readonly WpfPlot _wpfPlot;

    public RatingFrequencyPiePlot(WpfPlot wpfPlot)
    {
        _wpfPlot = wpfPlot;
    }

    public void PlotRatingFrequencies(Dictionary<int, int> frequencies)
    {
        var labels = frequencies.Keys.Select(e => "Rating " + e).ToArray();
        var values = frequencies.Values.Select(e => (double)e).ToArray();
        _wpfPlot.Plot.Clear();
        PlotPiePlot(values, labels);
    }

    private void PlotPiePlot(double[] values, string[] labels)
    {
        var pie = _wpfPlot.Plot.AddPie(values);
        Format(pie, labels);
        _wpfPlot.Refresh();
        ZoomToFitData();
    }

    private void ZoomToFitData()
    {
        _wpfPlot.Plot.AxisAuto();
    }

    private static void Format(PiePlot pie, string[] labels)
    {
        pie.SliceLabels = labels;
        pie.ShowLabels = true;
        pie.ShowValues = true;
    }
}