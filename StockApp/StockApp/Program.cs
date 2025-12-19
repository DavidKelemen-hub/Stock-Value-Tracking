using System;
using System.IO;
using System.Net.Http.Json;
using System.Runtime.InteropServices.JavaScript;
using System.Text.Json;
using System.Text.Json.Serialization;
using JsonSerializer = System.Text.Json.JsonSerializer;

public class Program
{
    // Create a string array with the lines of text
    static string api_key = "X0637BX0DJD9U0K6";
    static string symbol;
    static string url = $"https://www.alphavantage.co/query?function=TIME_SERIES_DAILY&symbol=&outputsize=compact&&apikey=";

    public class Root
    {
        [JsonPropertyName("Time Series (Daily)")]
        public Dictionary<string, DailyEntries> TimeSeriesDaily { get; set; }
    }

    public class DailyEntries
    {
        [JsonPropertyName("1. open")]
        public string Open { get; set; }

        [JsonPropertyName("2. high")]
        public string High { get; set; }

        [JsonPropertyName("3. low")]
        public string Low { get; set; }

        [JsonPropertyName("4. close")]
        public string Close { get; set; }

        [JsonPropertyName("5. volume")]
        public string Volume { get; set; }
    }


    public static async Task Main(string[] args)
    {
        symbol = "";
        while (!symbol.Equals("q"))
        {
            Console.WriteLine("Enter stock symbol:");
            symbol = Console.ReadLine();

            url = String.Format("https://www.alphavantage.co/query?function=TIME_SERIES_DAILY&symbol={0}&outputsize=compact&&apikey={1}", symbol, api_key);

            HttpClient client = new()
            {
                BaseAddress = new Uri(url)
            };

            HttpResponseMessage response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                var root = JsonSerializer.Deserialize<Root>(jsonResponse);
                var dailydata = root.TimeSeriesDaily;

                foreach (KeyValuePair<string, DailyEntries> item in dailydata)
                {
                    Console.WriteLine(string.Format("Closing value on {0} is {1}", item.Key, item.Value.Close));
                }
            }
            else
            {
                Console.WriteLine("Error: " + response.StatusCode);
            }
        }
    }
}
