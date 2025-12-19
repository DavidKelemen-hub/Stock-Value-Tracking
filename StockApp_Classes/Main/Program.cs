using StockApp_Classes.Services;
using StockApp_Classes.Processing;

class Program
{
    static async Task Main(string[] args)
    {
        StockService stockService = new StockService();
        Processing processing = new Processing();


        var dailyData = await stockService.GetTimeSeriesDataAsync("MSFT");

        double highestPrice = processing.GetHighestPrice(dailyData);

        Console.WriteLine($"The highest price for MSFT is: {highestPrice}");

        
    }
}