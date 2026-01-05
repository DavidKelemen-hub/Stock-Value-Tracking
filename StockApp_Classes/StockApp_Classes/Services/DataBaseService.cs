using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Dapper;
using StockApp_Classes.Models;

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
            const string queryString = "SELECT Name, Symbol FROM Company";

            using (var connection = new SqlConnection(this.connectionString))
            {
                var result = connection.Query<Company>(queryString).ToList();
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

        public List<DailyEntry> GetStockEntriesBetweenDates(string symbol, DateTime startDate, DateTime endDate)
        {
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

        public double GetClosePriceOnDate(string symbol, string date)
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


    }
}
