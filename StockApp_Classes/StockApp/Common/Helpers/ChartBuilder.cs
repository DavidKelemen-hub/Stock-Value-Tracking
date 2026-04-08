using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using SkiaSharp;
using StockApp.Domain.Models;
using System.Diagnostics;
using Axis = LiveChartsCore.SkiaSharpView.Axis;
using Chart = StockApp.Domain.Models.Chart;

namespace StockApp.Common.Helpers
{
    public class ChartBuilder
    {
        private List<DailyEntry> dailyEntries;

        public ChartBuilder(List<DailyEntry> data)
        {
            dailyEntries = data;
        }

        public Chart LoadChartData()
        {
            Chart chart = new();
            var prices = dailyEntries.Select(d => d.ClosePrice).ToArray();
            string dateFormat = DateTimeHelper.GetDateFormat(dailyEntries.Select(d => d.TradeDate).ToList());
            var labels = dailyEntries.Select(d => d.TradeDate.ToString(dateFormat)).ToList();

            var priceVariation = prices.Last() - prices.First();
            bool isPositiveTrend = priceVariation >= 0 ? true : false;

            var strokeColor = isPositiveTrend ? SKColor.Parse("#43A047") : SKColor.Parse("#E53935");
            var fillColor = isPositiveTrend ? new SKColor(67, 160, 71, 80) : new SKColor(229, 57, 53, 80);

            chart.ChartSeries = new ISeries[]
            {
                new LineSeries<double>
                {
                    Values = prices,
                    Fill = new LinearGradientPaint(
                        fillColor,   
                        new SKColor(255, 245, 240, 0)),   
                    Stroke = new SolidColorPaint(strokeColor) { StrokeThickness = 2 },
                    GeometrySize = 0,
                    LineSmoothness = 0.5
                }
            };

            chart.XAxes = new Axis[]
            {
                new Axis
                {
                    Labels = labels,
                    LabelsPaint = new SolidColorPaint(SKColor.Parse("#AAAAAA")),
                    SeparatorsPaint = null,
                    TextSize = 11,
                    MinLimit = 0,
                    MaxLimit = dailyEntries.Count - 1
                }
            };

            chart.YAxes = new Axis[]
            {
                new Axis
                {
                    LabelsPaint = new SolidColorPaint(SKColor.Parse("#AAAAAA")),
                    SeparatorsPaint = new SolidColorPaint(SKColor.Parse("#E8E0DB")) { StrokeThickness = 1 },
                    TextSize = 11
                }
            };

            return chart;
         }
        }
}
