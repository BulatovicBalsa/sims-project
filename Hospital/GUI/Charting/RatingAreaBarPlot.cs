using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using ScottPlot;
using ScottPlot.Plottable;

namespace Hospital.GUI.Charting;

public class RatingAreaBarPlot : ICategoryPlot
{
    private const double MaxRating = 5;
    private const double XMargin = 0.3;
    private readonly WpfPlot _wpfPlot;

    public RatingAreaBarPlot(WpfPlot wpfPlot)
    {
        _wpfPlot = wpfPlot;
    }

    public void Plot(Dictionary<string, double> averageRatingByArea)
    {
        var values = averageRatingByArea.Values.ToArray();
        var labels = averageRatingByArea.Keys.ToArray();
        _wpfPlot.Plot.Clear();
        PlotBarPlot(values, labels);
    }

    private void PlotBarPlot(double[] values, string[] labels)
    {
        var bar = _wpfPlot.Plot.AddBar(values);
        _wpfPlot.Plot.YTicks(labels);
        Format(bar);
        ZoomToFitScale();
        ZoomToFitCategories();
        _wpfPlot.Refresh();
    }

    private void Format(BarPlotBase bar)
    {
        bar.Orientation = Orientation.Horizontal;
        bar.ShowValuesAboveBars = true;
        bar.ValueFormatter = d => Math.Round(d, 2).ToString(CultureInfo.InvariantCulture);
        _wpfPlot.Plot.Title("Average rating by area");
    }

    private void ZoomToFitCategories()
    {
        _wpfPlot.Plot.AxisAutoY();
    }

    private void ZoomToFitScale()
    {
        _wpfPlot.Plot.SetAxisLimitsX(0, MaxRating + XMargin);
    }
}