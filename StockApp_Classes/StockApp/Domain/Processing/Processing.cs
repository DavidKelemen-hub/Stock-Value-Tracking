using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StockApp.Common.Helpers;
using StockApp.Domain.Models;
using StockApp.Infrastructure.DataAccess;

namespace StockApp.Domain.Processing
{
    public interface IProcessing
    {
        public Task<List<Company>> GetAllCompanies();
        public Task<List<CompanyPerformance>> GetTopPerformingCompanies(string range);
        public Task<List<CompanyPerformance>> GetLowestPerformingCompanies(string range);
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
            NullEntryHelper.SanitizeInput(result);

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

        public async Task<List<Company>> GetAllCompanies()
        {
            return await dbService.GetAllCompanies();
        }

        public async Task<List<CompanyPerformance>> GetTopPerformingCompanies(string range)
        {
            return await dbService.GetTopPerformingCompanies(range);
        }

        public async Task<List<CompanyPerformance>> GetLowestPerformingCompanies(string range)
        {

            return await dbService.GetLowestPerformingCompanies(range);
        }
    }
}

