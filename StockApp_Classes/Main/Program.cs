using StockApp_Classes.Services;
using StockApp_Classes.Processing;

class Program
{
    static async Task Main(string[] args)
    {
        string filePath = "C:\\Repos\\Stock-Value-Tracking\\StockApp_Classes\\StockApp_Classes\\Resources\\sp100.json";
        StockService stockService = new StockService();
        Processing processing = new Processing();
        SymbolService symbolService = new SymbolService(filePath);

        var symbols = symbolService.GetAllSymbols();


        foreach(var item in symbols)
        {
            
            var data = await stockService.GetTimeSeriesDataAsync(item.Symbol);
            var highestPrice = processing.GetHighestClosingPrice(data);
            var highestPriceDate = processing.GetHighestClosingPriceDate(data);
            Console.WriteLine($"{item.Name} - {item.Symbol} Highest Price: {highestPrice} on {highestPriceDate}");
            Thread.Sleep(12000); // To respect API rate limits
        }

        
    }
}