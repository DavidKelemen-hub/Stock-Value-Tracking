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
    string[] lines = { "First line", "Second line", "Third line" };
    string docPath = "C:\\Users\\K. David\\source\\repos\\ConsoleApp1\\ConsoleApp1\\output\\output.txt";
    static string api_key = "X0637BX0DJD9U0K6";
    static string debugdata = new string("");
    static string url = $"https://www.alphavantage.co/query?function=TIME_SERIES_DAILY&symbol=MSFT&outputsize=compact&&apikey={api_key}";

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
        HttpClient client = new()
        {
            BaseAddress = new Uri(url)
        };

        HttpResponseMessage response = await client.GetAsync(url);

        if (response.IsSuccessStatusCode)
        {
            string jsonResponse = await response.Content.ReadAsStringAsync();
            //Console.WriteLine(jsonResponse); //debug purpose
            var root = JsonSerializer.Deserialize<Root>(jsonResponse);
            var dailydata = root.TimeSeriesDaily;

            foreach (KeyValuePair<string, DailyEntries> item in dailydata)
            {
                Console.WriteLine(string.Format("Closing value on {0} is {1}", item.Key, item.Value.Close));
            }
        }






    }
}
