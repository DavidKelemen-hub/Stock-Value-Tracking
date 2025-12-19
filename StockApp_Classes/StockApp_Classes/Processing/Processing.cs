using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StockApp_Classes.Models;

namespace StockApp_Classes.Processing
{

    public class Processing
    {
        
        public double GetHighestPrice(Dictionary<string, DailyEntry> data)
        {
            return data.Max(x => Convert.ToDouble(x.Value.High));
        }

        public double GetLowestPrice(Dictionary<string, DailyEntry> data)
        {
            return data.Min(x => Convert.ToDouble(x.Value.Low));
        }

        public double GetHighestClosingPrice(Dictionary<string, DailyEntry> data)
        {
            return data.Max(x => Convert.ToDouble(x.Value.Close));
        }

        public double GetLowestClosingPrice(Dictionary<string, DailyEntry> data)
        {
            return data.Min(x => Convert.ToDouble(x.Value.Close));
        }

        public double GetHighestOpeningPrice(Dictionary<string, DailyEntry> data)
        {
            return data.Max(x => Convert.ToDouble(x.Value.Open));
        }

        public double GetLowestOpeningPrice(Dictionary<string, DailyEntry> data)
        {
            return data.Min(x => Convert.ToDouble(x.Value.Open));
        }

        public double GetHighestVolume(Dictionary<string, DailyEntry> data)
        {
            return data.Max(x => Convert.ToDouble(x.Value.Volume));
        }

        public double GetLowestVolume(Dictionary<string, DailyEntry> data)
        {
            return data.Min(x => Convert.ToDouble(x.Value.Volume));
        }

        public string GetHighestPriceDate(Dictionary<string, DailyEntry> data)
        {
            var highest = data.OrderByDescending(x => Convert.ToDouble(x.Value.High)).First();
            return highest.Key;
        }

        public string GetLowestPriceDate(Dictionary<string, DailyEntry> data)
        {
            var lowest = data.OrderBy(x => Convert.ToDouble(x.Value.Low)).First();
            return lowest.Key;
        }

    }
}

