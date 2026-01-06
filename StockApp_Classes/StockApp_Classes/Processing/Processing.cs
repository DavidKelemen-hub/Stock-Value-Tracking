using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StockApp_Classes.Models;
using StockApp_Classes.Services;

namespace StockApp_Classes.Processing
{
    public class Processing
    {
        DataBaseService dbService { get; set; }

        public Processing()
        {
            dbService = new DataBaseService(System.Configuration.ConfigurationManager.ConnectionStrings["dbconnection"].ConnectionString);
        }

        public double GetPriceVariation(string symbol, string range)
        {
            double percentageVariation = 0.0;
            
            var stockEntries = GetStockEntriesBetweenDates(symbol, range);

            double startPrice = stockEntries.First().ClosePrice;
            double endPrice = stockEntries.Last().ClosePrice;



            percentageVariation = ((endPrice - startPrice) * 100) / startPrice;

            return Math.Round(percentageVariation,2);
        }

        public List<Company> GetAllCompanies()
        {
            return dbService.GetAllCompanies();
        }

        public List<DailyEntry> GetStockEntriesBetweenDates(string symbol, string range)
        {
            return dbService.GetStockEntriesBetweenDates(symbol, range);
        }

    }
}

