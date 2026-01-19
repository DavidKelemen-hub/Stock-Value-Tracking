using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StockApp.Domain.Models;
using StockApp.Infrastructure.DataAccess;

namespace StockApp.Domain.Processing
{
    public interface IProcessing
    {
        public double GetHighestPriceInRange(string symbol, string range);
        public double GetLowestPriceInRange(string symbol, string range);
        public double GetPercentageVariationInRange(string symbol, string range);
        public double GetPriceVariationInRange(string symbol, string range);
        public string GetRangeDescription(double priceVariation, string range);
        public List<Company> GetAllCompanies();
        public List<CompanyPerformance> GetTopPerformingCompanies(string range);
        public List<CompanyPerformance> GetLowestPerformingCompanies(string range);
        public List<DailyEntry> GetStockEntriesBetweenDates(string symbol, string range);
        public double GetCurrentPrice(string symbol);
    }


    public class Processing : IProcessing
    {
        private readonly IDataBaseService dbService;

        public Processing(IDataBaseService dbService)
        {
            this.dbService = dbService;
        }

        public double GetHighestPriceInRange(string symbol, string range)
        {
            var stockEntries = GetStockEntriesBetweenDates(symbol, range);
            return stockEntries.Max(entry => entry.HighPrice);
        }

        public double GetLowestPriceInRange(string symbol, string range)
        {
            var stockEntries = GetStockEntriesBetweenDates(symbol, range);
            return stockEntries.Min(entry => entry.LowPrice);
        }

        public double GetPercentageVariationInRange(string symbol, string range)
        {
            double percentageVariation = 0.0;
            var stockEntries = GetStockEntriesBetweenDates(symbol, range);

            double startPrice = stockEntries.First().ClosePrice;
            double endPrice = stockEntries.Last().ClosePrice;

            percentageVariation = (endPrice - startPrice) * 100 / startPrice;
            return Math.Round(percentageVariation, 2);
        }

        public List<Company> GetAllCompanies()
        {
            return dbService.GetAllCompanies();
        }

        public List<CompanyPerformance> GetTopPerformingCompanies(string range)
        {
            return dbService.GetTopPerformingCompanies(range); ;
        }

        public List<CompanyPerformance> GetLowestPerformingCompanies(string range)
        {

            return dbService.GetLowestPerformingCompanies(range);
        }

        public List<DailyEntry> GetStockEntriesBetweenDates(string symbol, string range)
        {
            return dbService.GetStockEntriesBetweenDates(symbol, range);
        }

        public double GetCurrentPrice(string symbol)
        {
            return dbService.GetLatestClosePrice(symbol);
        }

        public double GetPriceVariationInRange(string symbol, string range)
        {
            List<DailyEntry> stockEntries = GetStockEntriesBetweenDates(symbol, range);
            double priceVariation = stockEntries.Last().ClosePrice - stockEntries.First().ClosePrice;
            return Math.Round(priceVariation, 2);
        }

        public string GetRangeDescription(double priceVariation,string range)
        {
            string description = range switch
            {
                "5D" => "past 5 days",
                "1M" => "past month",
                "3M" => "past 3 months",
                "6M" => "past 6 months",
                "YTD" => "year to date",
                "1Y" => "past year",
                "5Y" => "past 5 years",
                "Max" => "all time"
            };
            
            string trendArrow = priceVariation >= 0 ? "▲ " : "▼ ";

            return string.Concat(trendArrow, description);

        }

    }
}

