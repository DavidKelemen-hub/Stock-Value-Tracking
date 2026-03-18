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
        public List<Company> GetAllCompanies();
        public List<CompanyPerformance> GetTopPerformingCompanies(string range);
        public List<CompanyPerformance> GetLowestPerformingCompanies(string range);
        public IndividualStockData GetIndividualStockData(string symbol, string range);
    }


    public class Processing : IProcessing
    {
        private readonly IDataBaseService dbService;

        public Processing(IDataBaseService dbService)
        {
            this.dbService = dbService;
        }

        public List<DailyEntry> GetStockEntriesBetweenDates(string symbol, string range)
        {
            return dbService.GetStockEntriesBetweenDates(symbol, range);
        }

        public IndividualStockData GetIndividualStockData(string symbol, string range)
        {
            var result = GetStockEntriesBetweenDates(symbol, range);

            return new IndividualStockData
            {
                DailyValues = result,
                CurrentPrice = result.Last().ClosePrice,
                PriceVariation = result.Last().ClosePrice - result.First().ClosePrice,
                PercentageVariation = (result.Last().ClosePrice - result.First().ClosePrice) * 100 / result.First().ClosePrice,
                HighestPrice = result.Max(entry => entry.HighPrice),
                LowestPrice = result.Min(entry => entry.LowPrice)
            };
            
        }

        public List<Company> GetAllCompanies()
        {
            return dbService.GetAllCompanies();
        }

        public List<CompanyPerformance> GetTopPerformingCompanies(string range)
        {
            return dbService.GetTopPerformingCompanies(range);
        }

        public List<CompanyPerformance> GetLowestPerformingCompanies(string range)
        {

            return dbService.GetLowestPerformingCompanies(range);
        }
    }
}

