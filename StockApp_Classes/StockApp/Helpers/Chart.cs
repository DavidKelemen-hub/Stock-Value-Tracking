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
using System.Windows.Media;

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

        public PlotModel LoadChartData(string range, List<DailyEntry> dailyEntries, int sign)
        {
            this.chartData.Series.Clear();
            this.chartData.Axes.Clear();
            string dateFormat = string.Empty;
            int majorStep = 0;
            var intervalType = DateTimeIntervalType.Days;

            switch (range)
            {
                case "5D":
                    this.chartData.Title = this.chartData.Title + " - Last 5 Days";
                    dateFormat = "MMM dd";
                    majorStep = 1;
                    intervalType = DateTimeIntervalType.Days;
                    break;
                case "1M":
                    this.chartData.Title = this.chartData.Title + " - Last Month";
                    dateFormat = "MMM dd";
                    majorStep = 8;
                    intervalType = DateTimeIntervalType.Days;
                    break;
                case "6M":
                    this.chartData.Title = this.chartData.Title + " - Last 6 Months";
                    dateFormat = "MMM yyyy";
                    majorStep = 48;
                    intervalType = DateTimeIntervalType.Months;
                    break;
                case "YTD":
                    this.chartData.Title = this.chartData.Title + " - Year to Date";
                    dateFormat = "dd MMM";
                    majorStep = 1;
                    intervalType = DateTimeIntervalType.Auto;
                    break;
                case "1Y":
                    this.chartData.Title = this.chartData.Title + " - Last Year";
                    dateFormat = "MMM yyyy";
                    majorStep = 96;
                    intervalType = DateTimeIntervalType.Years;
                    break;
                case "5Y":
                    this.chartData.Title = this.chartData.Title + " - Last 5 Years";
                    dateFormat = "yyyy";
                    majorStep = 480;
                    intervalType = DateTimeIntervalType.Years;
                    break;
                case "Max":
                    this.chartData.Title = this.chartData.Title + " - All time";
                    dateFormat = "yyyy";
                    majorStep = 2000;
                    intervalType = DateTimeIntervalType.Years;
                    break;
            }
            this.chartData.Axes.Add(new DateTimeAxis
            {
                Position = AxisPosition.Bottom,
                StringFormat = dateFormat,
                AxislineStyle = LineStyle.Solid,
                MajorStep = majorStep,
                //MinorTickSize = 2,
                IsZoomEnabled = false,
                IsPanEnabled = false,
                IntervalType = intervalType

            }); 

            this.chartData.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
                AxislineStyle = LineStyle.Dot,
                MajorTickSize = 7,
                IsZoomEnabled = false,
                IsPanEnabled = false,
            });


            var series = new LineSeries();
            if(sign == 0 || sign == 1)
            {
                series.Color = OxyColors.LimeGreen;
            }
            else
            {
                series.Color = OxyColors.IndianRed;
            }
                foreach (var p in dailyEntries)
                    series.Points.Add(DateTimeAxis.CreateDataPoint(p.TradeDate, p.ClosePrice));

            this.chartData.Series.Add(series);
            this.chartData.InvalidatePlot(true);

            return this.chartData;
        }
    }
}
