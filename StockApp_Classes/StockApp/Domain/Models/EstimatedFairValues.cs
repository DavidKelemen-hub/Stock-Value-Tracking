using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockApp.Domain.Models
{
    public class EstimatedFairValues
    {
        public decimal? GrahamFairValue { get; set; }
        public decimal? DiscountedCashFlow { get; set; }
        public decimal? PEBasedFairValue { get; set; }
        public decimal? EbitdaBasedFairValue{get;set;}
        public decimal? DividendDiscountModelFairValue { get; set; }
    }
}
