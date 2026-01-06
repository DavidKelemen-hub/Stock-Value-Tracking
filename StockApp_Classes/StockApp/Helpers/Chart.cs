using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using StockApp_Classes.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockApp.Helpers
{
    public class ChartBuilder
    {

        public PlotModel chartData;

        public ChartBuilder(string title)
        { 
            this.chartData = new PlotModel();
            this.chartData.Title = $"Stock Price for {title}";
        }

        public PlotModel LoadChartData(string range, List<DailyEntry> dailyEntries)
        {
            this.chartData.Series.Clear();
            this.chartData.Axes.Clear();

            this.chartData.Axes.Add(new DateTimeAxis
            {
                Position = AxisPosition.Bottom,
                StringFormat = "yyyy-MM-dd",
                AxislineStyle = LineStyle.Solid,
                MajorTickSize = 2,
                MinorTickSize = 2,
                IsZoomEnabled = false,
                IsPanEnabled = false

            });

            this.chartData.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
                AxislineStyle = LineStyle.LongDashDotDot,
                MajorTickSize = 7,
                MinorTickSize = 4,
                IsZoomEnabled = false,
                IsPanEnabled = false
            });


            var s = new LineSeries();
            foreach (var p in dailyEntries)
                s.Points.Add(DateTimeAxis.CreateDataPoint(p.TradeDate, p.ClosePrice));

            this.chartData.Series.Add(s);
            this.chartData.InvalidatePlot(true);

            return this.chartData;
        }
    }
}
