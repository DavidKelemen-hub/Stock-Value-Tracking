using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockApp.Domain.Models
{
    public class EstimatedFairValues
    {
        public string? GrahamFairValue { get; set; }
        public string? DiscountedCashFlow { get; set; }
        public string? PEBasedFairValue { get; set; }
        public string? EbitdaBasedFairValue{get;set;}
        public string? DividendDiscountModelFairValue { get; set; }
    }
}
