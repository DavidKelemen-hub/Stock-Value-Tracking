using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockApp.Domain.Models
{
    public class IndividualStockData
    {
        public List<DailyEntry>? DailyValues { get; set; }
        public decimal? CurrentPrice { get; set; }
        public decimal? PriceVariation { get; set; }
        public decimal? PercentageVariation { get; set; }
        public decimal? HighestPrice { get; set; }
        public decimal? LowestPrice { get; set; }
    }
}
