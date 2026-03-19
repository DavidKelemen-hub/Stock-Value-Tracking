using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockApp.Domain.Models
{
    public class IndividualStockData
    {
        public List<DailyEntry> DailyValues { get; set; }
        public double CurrentPrice { get; set; }
        public double PriceVariation { get; set; }
        public double PercentageVariation { get; set; }
        public double HighestPrice { get; set; }
        public double LowestPrice { get; set; }
    }
}
