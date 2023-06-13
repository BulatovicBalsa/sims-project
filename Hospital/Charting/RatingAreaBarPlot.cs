using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using ScottPlot;

namespace Hospital.Charting;

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
        var bar = _wpfPlot.Plot.AddBar(values);
        _wpfPlot.Plot.YTicks(labels);
        _wpfPlot.Plot.SetAxisLimitsX(0, MaxRating + XMargin);
        bar.Orientation = Orientation.Horizontal;
        bar.ShowValuesAboveBars = true;
        bar.ValueFormatter = d => Math.Round(d, 2).ToString(CultureInfo.InvariantCulture);
        _wpfPlot.Plot.Title("Average rating by area");
        _wpfPlot.Refresh();
        _wpfPlot.Plot.AxisAutoY();
    }
}