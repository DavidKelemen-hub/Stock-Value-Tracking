using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockApp.Domain.Models
{
    public class Chart
    {
        public ISeries[] ChartSeries { get; set; } 
        public Axis[] XAxes { get; set; }
        public Axis[] YAxes { get; set; }
    }
}
