using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using StockApp_Classes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockApp.ViewModels
{
    public class PlotViewModel
    {
        public PlotModel PlotModel { get; } = new PlotModel();

        public void LoadSeries(List<DailyEntry> data)
        {
            PlotModel.Series.Clear();
            PlotModel.Axes.Clear();

            PlotModel.Axes.Add(new DateTimeAxis { Position = AxisPosition.Bottom, StringFormat = "yyyy-MM-dd" });
            PlotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Left });

            var s = new LineSeries();
            foreach (var p in data)
                s.Points.Add(DateTimeAxis.CreateDataPoint(p.TradeDate, p.ClosePrice));

            PlotModel.Series.Add(s);
            PlotModel.InvalidatePlot(true);
        }
    }
}
