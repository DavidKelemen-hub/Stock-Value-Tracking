using StockApp.Models;
using StockApp.Helpers;
using Microsoft.Data.SqlClient;
using Dapper;

namespace StockApp.Services
{
    public class DataBaseService
    {
        private readonly string connectionString;
        public DataBaseService(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public List<Company> GetAllCompanies()
        {
            const string queryString = "SELECT Name, Symbol FROM Company ORDER BY Symbol ASC";
                                       
            using (var connection = new SqlConnection(this.connectionString))
            {
                var result = connection.Query<Company>(queryString).ToList();
                return result;
            }
        }

        public List<CompanyPerformance> GetTopPerformingCompanies(string range)
        {
            
            DateTime end = DateTime.Today;
            DateTime start;
            DateTimeHelper dtHelper = new DateTimeHelper();
            if (range == "5D")
            {
                start = GetLast5TradingDays().Last().TradeDate;
            }
            else
            {
                start = dtHelper.GetStartDate(range);
            }
                
            const string queryString = @"EXEC GetTopPerformingCompanies @startDate=@start, @endDate=@end";

            using (var connection = new SqlConnection(this.connectionString))
            {
                var result = connection.Query<CompanyPerformance>(queryString, new { start, end } ).ToList();
                return result;
            }
        }

        public List<CompanyPerformance> GetLowestPerformingCompanies(string range)
        {
            DateTime end = DateTime.Today;
            DateTimeHelper dtHelper = new DateTimeHelper();
            DateTime start = dtHelper.GetStartDate(range);

            string queryString = @"EXEC GetLowestPerformingCompanies @startDate=@start, @endDate=@end";

            using (var connection = new SqlConnection(this.connectionString))
            {
                var result = connection.Query<CompanyPerformance>(queryString,new {start, end}).ToList();
                return result;
            }
        }

        public int GetCompanyIDFromSymbol(string symbol)
        {
            const string queryString = "SELECT StockID FROM Company WHERE Symbol = @symbol";

            using (var connection = new SqlConnection(this.connectionString))
            {
                var result = connection.QuerySingleOrDefault<int>(queryString, new { symbol });
                return result;
            }
        }

        public int GetCompanyIDFromName(string name)
        {
            const string queryString = "SELECT StockID FROM Company WHERE Name = @name";

            using (var connection = new SqlConnection(this.connectionString))
            {
                var result = connection.QuerySingleOrDefault<int>(queryString, new { name });
                return result;
            }
        }

        public List<DailyEntry> GetCompleteStockData(string symbol)
        {
            var stockID = GetCompanyIDFromSymbol(symbol);
            const string queryString = "SELECT * " +
                                       "FROM DailyPrices WHERE StockID = @stockID";

            using (var connection = new SqlConnection(this.connectionString))
            {
                var result = connection.Query<DailyEntry>(queryString, new { stockID });
                return result.ToList();
            }
        }

        public List<DailyEntry> GetLast5TradingDays()
        {
            
            const string queryString = "SELECT DISTINCT TOP 5 TradeDate FROM DailyPrices ORDER BY TradeDate DESC";

            using (var connection = new SqlConnection(this.connectionString))
            {
                var result = connection.Query<DailyEntry>(queryString);
                return result.ToList();
            }
        }

        public List<DailyEntry> GetStockEntriesBetweenDates(string symbol, string range)
        {
            DateTime endDate = DateTime.Today;
            DateTimeHelper dtHelper = new DateTimeHelper(); 

            if(range == "Max")
            {
                var result = GetCompleteStockData(symbol);
                return result.ToList();
            }
            else
            {
                DateTime startDate = dtHelper.GetStartDate(range);
                var stockID = GetCompanyIDFromSymbol(symbol);

                const string queryString = "SELECT * " +
                                           "FROM DailyPrices WHERE StockID = @stockID " +
                                           "AND TradeDate BETWEEN @startDate AND @endDate";

                using (var connection = new SqlConnection(this.connectionString))
                {
                    var result = connection.Query<DailyEntry>(queryString, new { stockID, startDate, endDate });
                    return result.ToList();
                }
            }
        }

        public double GetClosePriceOnDate(string symbol, DateTime date)
        {
            var stockID = GetCompanyIDFromSymbol(symbol);
            const string queryString = "SELECT ClosePrice " +
                                       "FROM DailyPrices WHERE StockID = @stockID " +
                                       "AND TradeDate = @date";
            using (var connection = new SqlConnection(this.connectionString))
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
            using (var connection = new SqlConnection(this.connectionString))
            {
                var result = connection.QuerySingleOrDefault<double>(queryString, new { stockID });
                return result;
            }
        }
    }
}
