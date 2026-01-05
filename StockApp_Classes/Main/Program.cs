using StockApp_Classes.Services;
using StockApp_Classes.Processing;
using System.Runtime.Serialization;
using System;
using System.Configuration;

class Program
{
    static async Task Main(string[] args)
    {
        var connectionString =
        ConfigurationManager.ConnectionStrings["dbconnection"].ConnectionString;


        DataBaseService service = new DataBaseService(connectionString);

       var result = service.GetStockEntriesBetweenDates("AAPL", "2025.01.01","2025.07.01");

        foreach (var entry in result)
        {
            Console.WriteLine($"{entry.TradeDate} - Open: {entry.OpenPrice}, Close: {entry.ClosePrice}");
        }
       // Console.WriteLine(result);


    }
}