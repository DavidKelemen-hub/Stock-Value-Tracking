using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockApp.Appl.DTO
{
    public class FinancialStatementDTO
    {
        public string? TrailingEPS { get; set; } = "N/A";
        public string? ForwardEPS { get; set; } = "N/A";
        public string? BookValue { get; set; } = "N/A";
        public string? GrossMargins { get; set; } = "N/A";
        public string? OperatingMargins { get; set; } = "N/A";
        public string? Ebitda { get; set; } = "N/A";
        public string? RevenueGrowth { get; set; } = "N/A";
        public string? EarningsGrowth { get; set; } = "N/A";
        public string? TotalDebt { get; set; } = "N/A";
        public string? NetCashPosition { get; set; } = "N/A";
        public string? FreeCashFlow { get; set; } = "N/A";
        public string? CurrentRatio { get; set; } = "N/A";
        public string? DividendRate { get; set; } = "N/A";
        public string? DividendYield { get; set; } = "N/A";
        public string? Beta { get; set; } = "N/A";
        public string? SharesOutstanding { get; set; } = "N/A";
    }
}
