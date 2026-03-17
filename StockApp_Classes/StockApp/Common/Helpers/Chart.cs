using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using StockApp.Domain.Models;

namespace StockApp.Common.Helpers
{
    public class ChartBuilder
    {

        public PlotModel chartData;

        public ChartBuilder(string title)
        { 
            chartData = new PlotModel();
            chartData.Title = $"Stock Price for {title}";
        }

        public PlotModel LoadChartData(string range, List<DailyEntry> dailyEntries, int sign)
        {
            chartData.Series.Clear();
            chartData.Axes.Clear();
            string dateFormat = string.Empty;
            int majorStep = 0;
            var intervalType = DateTimeIntervalType.Days;

            switch (range)
            {
                case "5D":
                    chartData.Title = chartData.Title + " - Last 5 Days";
                    dateFormat = "MMM dd";
                    majorStep = 1;
                    intervalType = DateTimeIntervalType.Days;
                    break;
                case "1M":
                    chartData.Title = chartData.Title + " - Last Month";
                    dateFormat = "MMM dd";
                    majorStep = 8;
                    intervalType = DateTimeIntervalType.Days;
                    break;
                case "6M":
                    chartData.Title = chartData.Title + " - Last 6 Months";
                    dateFormat = "MMM yyyy";
                    majorStep = 48;
                    intervalType = DateTimeIntervalType.Months;
                    break;
                case "YTD":
                    chartData.Title = chartData.Title + " - Year to Date";
                    dateFormat = "dd MMM";
                    majorStep = 10; //check this - as time passes, we have to shrink this
                    intervalType = DateTimeIntervalType.Auto;
                    break;
                case "1Y":
                    chartData.Title = chartData.Title + " - Last Year";
                    dateFormat = "MMM yyyy";
                    majorStep = 96;
                    intervalType = DateTimeIntervalType.Years;
                    break;
                case "5Y":
                    chartData.Title = chartData.Title + " - Last 5 Years";
                    dateFormat = "yyyy";
                    majorStep = 480;
                    intervalType = DateTimeIntervalType.Years;
                    break;
                case "Max":
                    chartData.Title = chartData.Title + " - All time";
                    dateFormat = "yyyy";
                    majorStep = 2000;
                    intervalType = DateTimeIntervalType.Years;
                    break;
            }
            chartData.Axes.Add(new DateTimeAxis
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

            chartData.Axes.Add(new LinearAxis
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

            chartData.Series.Add(series);
            chartData.InvalidatePlot(true);

            return chartData;
        }
    }
}
