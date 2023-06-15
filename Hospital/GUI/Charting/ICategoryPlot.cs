﻿using System.Collections.Generic;

namespace Hospital.GUI.Charting;

public interface ICategoryPlot
{
    public void Plot(Dictionary<string, double> categoricalData);
}