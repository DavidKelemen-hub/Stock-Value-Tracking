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

        public double GetOpenPriceByDate(Dictionary<string, DailyEntry> data, string date)
        {
            if (data.ContainsKey(date))
            {
                return Math.Round(Convert.ToDouble(data[date].Open), 2);
            }
            throw new ArgumentException("Date not found in data.");
        }

        public double GetClosePriceByDate(Dictionary<string, DailyEntry> data, string date)
        {
            if (data.ContainsKey(date))
            {
                return Math.Round(Convert.ToDouble(data[date].Close), 2);
            }
            throw new ArgumentException("Date not found in data.");
        }

        public double GetHighPriceByDate(Dictionary<string, DailyEntry> data, string date)
        {
            if (data.ContainsKey(date))
            {
                return Math.Round(Convert.ToDouble(data[date].High), 2);
            }
            throw new ArgumentException("Date not found in data.");
        }

        public double GetLowPriceByDate(Dictionary<string, DailyEntry> data, string date)
        {
            if (data.ContainsKey(date))
            {
                return Math.Round(Convert.ToDouble(data[date].Low), 2);
            }
            throw new ArgumentException("Date not found in data.");
        }

        public int GetVolumeByDate(Dictionary<string, DailyEntry> data, string date)
        {
            if (data.ContainsKey(date))
            {
                return Convert.ToInt32(data[date].Volume);
            }
            throw new ArgumentException("Date not found in data.");
        }

        public double GetAverageClosingPrice(Dictionary<string, DailyEntry> data)
        {
            return Math.Round(data.Average(x => Convert.ToDouble(x.Value.Close)), 2);
        }

        public double GetAverageOpeningPrice(Dictionary<string, DailyEntry> data)
        {
            return Math.Round(data.Average(x => Convert.ToDouble(x.Value.Open)), 2);
        }

        public double GetAverageHighPrice(Dictionary<string, DailyEntry> data)
        {
            return Math.Round(data.Average(x => Convert.ToDouble(x.Value.High)), 2);
        }

        public double GetAverageLowPrice(Dictionary<string, DailyEntry> data)
        {
            return Math.Round(data.Average(x => Convert.ToDouble(x.Value.Low)), 2);

        }

        public double GetPercentChange(Dictionary<string, DailyEntry> data, string startDate, string endDate)
        {
            if (data.ContainsKey(startDate) && data.ContainsKey(endDate))
            {
                double startPrice = Convert.ToDouble(data[startDate].Close);
                double endPrice = Convert.ToDouble(data[endDate].Close);
                return Math.Round(((endPrice - startPrice) / startPrice) * 100, 2);
            }
            throw new ArgumentException("One or both dates not found in data.");
        }
    }
}

