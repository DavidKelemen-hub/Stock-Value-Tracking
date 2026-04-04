using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockApp.Domain.Models
{
    public class FinancialStatement
    {
        public int FundamentalID { get; set; }
        public int StockID { get; set; }
        public float? TrailingEPS { get; set; }
        public float? ForwardEPS { get; set; }
        public float? BookValue { get; set; }
        public long? FreeCashFlow { get; set; }
        public float? EarningsGrowth { get; set; }
        public float? RevenueGrowth { get; set; }
        public long? SharesOutstanding { get; set; }
        public long? TotalDebt { get; set; }
        public long? TotalCash { get; set; }
        public long? EBITDA { get; set; }
        public float? DividendRate { get; set; }
        public float? DividendYield { get; set; }
        public float? DebtToEquity { get; set; }
        public float? ReturnOnEquity { get; set; }
        public float? ReturnOnAssets { get; set; }
        public float? CurrentRatio { get; set; }
        public float? GrossMargins { get; set; }
        public float? OperatingMargins { get; set; }
        public float? Beta { get; set; }
        public string? Sector { get; set; }
        public DateTime? LatestUpdate { get; set; }
    }
}
