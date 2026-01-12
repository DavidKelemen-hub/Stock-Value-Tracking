using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace StockApp_Classes.Models
{
    public class DailyEntriesCollection
    {
        public required List<DailyEntry> TimeSeriesDaily { get; set; }

    }

    public class CompanyPerformance
    {
        public string Symbol { get; set; }
        public string Name { get; set; }
        public double PriceChange { get; set; }
        public double PercentChange { get; set; }
        
    }

    public class DailyEntry
    {
        public int StockID { get; set; }
        public DateTime TradeDate { get; set; }
        public double OpenPrice { get; set; }
        public double HighPrice { get; set; }
        public double LowPrice { get; set; }
        public double ClosePrice { get; set; }
        public long Volume { get; set; }
    }

    public class Company
    {
        public int StockID { get; set; }
        public string Symbol { get; set; }
        public string Name { get; set; }
    }

   
}
