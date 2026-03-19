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
        public Task<IndividualStockData> GetIndividualStockData(string symbol, string range);
    }


    public class Processing : IProcessing
    {
        private readonly IDataBaseService dbService;

        public Processing(IDataBaseService dbService)
        {
            this.dbService = dbService;
        }


        public async Task<IndividualStockData> GetIndividualStockData(string symbol, string range)
        {
            var result = await dbService.GetStockEntriesBetweenDates(symbol, range);
            var First = result.First();
            var Last = result.Last();

            return new IndividualStockData
            {
                DailyValues = result,
                CurrentPrice = Last.ClosePrice,
                PriceVariation = Last.ClosePrice - First.ClosePrice,
                PercentageVariation = (Last.ClosePrice - First.ClosePrice) * 100 / First.ClosePrice,
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

