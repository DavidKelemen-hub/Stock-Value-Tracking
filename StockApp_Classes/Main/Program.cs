using StockApp_Classes.Services;
using StockApp_Classes.Processing;
using System.Runtime.Serialization;

class Program
{
    static async Task Main(string[] args)
    {
       const string connectionstring = "Data Source=localhost;Initial Catalog=StockData;Integrated Security=True;Trust Server Certificate=True";

        DataBaseService service = new DataBaseService(connectionstring);

       var result = service.GetCompanyIDFromName("AbbVie");

       //foreach (var entry in result)
       //{
       // Console.WriteLine($"{entry.TradeDate} - Open: {entry.OpenPrice}, Close: {entry.ClosePrice}");
       // }
        Console.WriteLine(result);


    }
}