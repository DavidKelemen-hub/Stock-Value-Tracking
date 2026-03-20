using Microsoft.Data.SqlClient;
using Dapper;
using StockApp.Domain.Models;
using StockApp.Common.Helpers;
using System.Runtime.Caching;

namespace StockApp.Infrastructure.DataAccess
{

    public interface IDataBaseService
    {
        public List<Company> GetAllCompanies();
        public Task<List<CompanyPerformance>> GetTopPerformingCompanies(string range);
        public Task<List<CompanyPerformance>> GetLowestPerformingCompanies(string range);
        public Task<List<DailyEntry>> GetCompleteStockData(string symbol);
        public Task<List<DailyEntry>> GetLast5TradingDays();
        public Task <List<DailyEntry>> GetStockEntriesBetweenDates(string symbol, string range);
        public double GetClosePriceOnDate(string symbol, DateTime date);
        public double GetLatestClosePrice(string symbol);
    }
    public class DataBaseService : IDataBaseService
    {
        private readonly string connectionString;
        private static readonly MemoryCache _cache = MemoryCache.Default;
        private readonly CacheItemPolicy policy = new CacheItemPolicy
        {
            AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(5)
        };
        public DataBaseService()
        {
            connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["dbconnection"].ConnectionString;
        }

        public List<Company> GetAllCompanies()
        {
            const string queryString = "SELECT Name, Symbol FROM Company ORDER BY Symbol ASC";
                                       
            using (var connection = new SqlConnection(connectionString))
            {
                var result = connection.Query<Company>(queryString).AsList();
                return result;
            }
        }

        public async Task<List<CompanyPerformance>> GetTopPerformingCompanies(string range)
        {
            
            DateTime end = DateTime.Today;
            DateTime start;
            
            if (range == "5D")
            {
                start = (await (GetLast5TradingDays())).Last().TradeDate;
            }
            else
            {
                start = DateTimeHelper.GetStartDate(range);
            }
                
            const string queryString = @"EXEC GetTopPerformingCompanies @startDate=@start, @endDate=@end";

            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                var result = (await connection.QueryAsync<CompanyPerformance>(queryString, new { start, end } )).AsList();

                return result;
            }
        }

        public async Task<List<CompanyPerformance>> GetLowestPerformingCompanies(string range)
        {
            DateTime end = DateTime.Today;
            DateTime start = DateTimeHelper.GetStartDate(range);

            string queryString = @"EXEC GetLowestPerformingCompanies @startDate=@start, @endDate=@end";

            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                return (await connection.QueryAsync<CompanyPerformance>(queryString,new {start, end})).AsList();
            }
        }

        public async Task<int> GetCompanyIDFromSymbol(string symbol)
        {
            if (_cache[symbol] is int cached) return cached;
            const string queryString = "SELECT StockID FROM Company WHERE Symbol = @symbol";

            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                var result = await connection.QuerySingleOrDefaultAsync<int>(queryString, new { symbol });
                
                _cache.Set(symbol,result, policy);
                return result;
            }
        }

        public async Task<List<DailyEntry>> GetCompleteStockData(string symbol)
        {
            var stockID = await GetCompanyIDFromSymbol(symbol);
            const string queryString = "SELECT * " +
                                       "FROM DailyPrices WHERE StockID = @stockID";

            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                var result = await connection.QueryAsync<DailyEntry>(queryString, new { stockID });

                return result.AsList();
            }
        }

        public async Task<List<DailyEntry>> GetLast5TradingDays()
        {
            
            const string queryString = "SELECT DISTINCT TOP 5 TradeDate FROM DailyPrices ORDER BY TradeDate DESC";

            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                return (await connection.QueryAsync<DailyEntry>(queryString)).AsList();
            }
        }

        public async Task<List<DailyEntry>> GetStockEntriesBetweenDates(string symbol, string range)
        {
            DateTime endDate = DateTime.Today;

            if(range == "Max")
            {
                return (await GetCompleteStockData(symbol)).AsList();
            }
            else
            {
                DateTime startDate = DateTimeHelper.GetStartDate(range);
                var stockID = await GetCompanyIDFromSymbol(symbol);

                const string queryString = "SELECT * " +
                                           "FROM DailyPrices WHERE StockID = @stockID " +
                                           "AND TradeDate BETWEEN @startDate AND @endDate";

                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    var result = await connection.QueryAsync<DailyEntry>(queryString, new { stockID, startDate, endDate });

                    return result.AsList();
                }
            }
        }

        public double GetClosePriceOnDate(string symbol, DateTime date)
        {
            var stockID = GetCompanyIDFromSymbol(symbol);
            const string queryString = "SELECT ClosePrice " +
                                       "FROM DailyPrices WHERE StockID = @stockID " +
                                       "AND TradeDate = @date";
            using (var connection = new SqlConnection(connectionString))
            {
                var result = connection.QuerySingleOrDefault<double>(queryString, new { stockID, date });
                return result;
            }
        }

        public double GetLatestClosePrice(string symbol)
        {
            var stockID = GetCompanyIDFromSymbol(symbol);
            const string queryString = "SELECT TOP 1 ClosePrice " +
                                       "FROM DailyPrices WHERE StockID = @stockID " +
                                       "ORDER BY TradeDate DESC";
            using (var connection = new SqlConnection(connectionString))
            {
                var result = connection.QuerySingleOrDefault<double>(queryString, new { stockID });
                return result;
            }
        }
    }
}
