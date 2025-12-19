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
        [JsonPropertyName("Time Series (Daily)")]
        public required Dictionary<string, DailyEntry> TimeSeriesDaily { get; set; }

    }

    public class DailyEntry
    {
        [JsonPropertyName("1. open")]
        public string Open { get; set; }

        [JsonPropertyName("2. high")]
        public string High { get; set; }

        [JsonPropertyName("3. low")]
        public string Low { get; set; }

        [JsonPropertyName("4. close")]
        public string Close { get; set; }

        [JsonPropertyName("5. volume")]
        public string Volume { get; set; }
    }
}
