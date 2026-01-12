using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Dapper;
using StockApp_Classes.Models;
using StockApp.Helpers;

namespace StockApp_Classes.Services
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

            DateTime endDate = DateTime.Today;
            DateTimeHelper dtHelper = new DateTimeHelper();
            DateTime startDate = dtHelper.GetStartDate(range);

            const string queryString = @"WITH bounds AS (
    SELECT
        dp.StockID,
        MIN(dp.TradeDate) AS StartTradeDate,
        MAX(dp.TradeDate) AS EndTradeDate
    FROM dbo.DailyPrices dp
    WHERE dp.TradeDate BETWEEN @startDate AND @endDate
    GROUP BY dp.StockID
),
var AS (
    SELECT
        c.StockID,
        c.Symbol,
        c.Name,
        s.ClosePrice AS StartClose,
        e.ClosePrice AS EndClose,
        ROUND((e.ClosePrice - s.ClosePrice),2) AS PriceChange,
        CASE 
            WHEN s.ClosePrice = 0 THEN NULL
            ELSE ROUND(((e.ClosePrice - s.ClosePrice) / s.ClosePrice) * 100,2)
        END AS PercentChange
    FROM bounds b
    JOIN dbo.DailyPrices s 
        ON s.StockID = b.StockID 
       AND s.TradeDate = b.StartTradeDate
    JOIN dbo.DailyPrices e 
        ON e.StockID = b.StockID 
       AND e.TradeDate = b.EndTradeDate
    JOIN dbo.Company c     
        ON c.StockID = b.StockID
)
SELECT TOP (10)
    Symbol,
    Name,
    PriceChange,
    PercentChange
FROM var
ORDER BY PercentChange DESC;
";
            using (var connection = new SqlConnection(this.connectionString))
            {
                var result = connection.Query<CompanyPerformance>(queryString, new { startDate, endDate } ).ToList();
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
