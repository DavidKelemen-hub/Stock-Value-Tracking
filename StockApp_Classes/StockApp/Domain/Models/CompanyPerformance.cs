using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockApp.Domain.Models
{
    public class CompanyPerformance
    {
        public string Symbol { get; set; }
        public string Name { get; set; }
        public double PriceChange { get; set; }
        public double PercentChange { get; set; }
    }
}
