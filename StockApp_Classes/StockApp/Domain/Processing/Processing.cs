using Dapper;
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
        public Task<EstimatedFairValues> GetEstimatedFairValues(string symbol);
        public Task<FinancialStatement> GetFinancialStatement(string symbol);
        public Task<Root> GetNewsFeed(string symbol, int size);
    }


    public class Processing : IProcessing
    {
        private readonly IDataBaseService dbService;
        private readonly IMessageService msgService;

        public Processing(IDataBaseService dbService, IMessageService msgService)
        {
            this.dbService = dbService;
            this.msgService = msgService;
        }

        public async Task<IndividualStockData> GetIndividualStockData(string symbol, string range)
        {
            var result = await dbService.GetStockEntriesBetweenDates(symbol, range).ConfigureAwait(false);
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
            return await dbService.GetAllCompanies().ConfigureAwait(false);
        }

        public async Task<List<CompanyPerformance>> GetTopPerformingCompanies(string range)
        {
            return await dbService.GetTopPerformingCompanies(range).ConfigureAwait(false);
        }

        public async Task<List<CompanyPerformance>> GetLowestPerformingCompanies(string range)
        {

            return await dbService.GetLowestPerformingCompanies(range).ConfigureAwait(false);
        }

        public async Task<EstimatedFairValues?> GetEstimatedFairValues(string symbol)
        {
            var statement = await dbService.GetFinancialStatement(symbol).ConfigureAwait(false);

            if (statement == null) return null;

            var sectorMedianPETask = dbService.GetSectorMedianPE(statement.Sector);
            var sectorMedianEV_EBITDATask = dbService.GetSectorMedianEV_EBITDA(statement.Sector);
            var riskFreeRateTask = dbService.GetRiskFreeRate();

            await Task.WhenAll(sectorMedianPETask, sectorMedianEV_EBITDATask, riskFreeRateTask);

            decimal? sectorMedianPE = sectorMedianPETask.Result;
            decimal? sectorMedianEV_EBITDA = sectorMedianEV_EBITDATask.Result;
            double? riskFreeRate = riskFreeRateTask.Result;

            return new EstimatedFairValues
            {
                GrahamFairValue = FairValueHelper.Graham_Value(statement),
                PEBasedFairValue = FairValueHelper.PE_Value(statement, sectorMedianPE),
                EbitdaBasedFairValue = FairValueHelper.EbitdaBased_Value(statement, sectorMedianEV_EBITDA),
                DividendDiscountModelFairValue = FairValueHelper.DividendDiscountModel_Value(statement, riskFreeRate)
            };
            
        }

        public async Task<FinancialStatement> GetFinancialStatement(string symbol)
        {
            return await dbService.GetFinancialStatement(symbol).ConfigureAwait(false);
        }

        public async Task<Root> GetNewsFeed(string symbol, int size)
        {
            return await msgService.GetNewsFeed(symbol, size).ConfigureAwait(false);
        }


    }
}

