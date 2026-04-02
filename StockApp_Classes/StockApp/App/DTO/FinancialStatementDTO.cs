using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockApp.Appl.DTO
{
    public class FinancialStatementDTO
    {
        public string? FreeCashFlow { get; set; }
        public string? ReturnOnEquity { get; set; }
        public string? DebtToEquity { get; set; }
        public string? GrossMargins { get; set; }
        public string? OperatingMargins { get; set; }
        public string? CurrentRatio { get; set; }
        public string? Ebitda { get; set; }
        public string? TrailingEPS { get; set; }
        public string? RevenueGrowth { get; set; }
        public string? NetCashPosition { get; set; }
    }
}
