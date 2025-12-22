using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using StockApp_Classes.Models;

namespace StockApp_Classes.Services
{

    public class StockService
    {
        private readonly string _apiKey;
        private readonly string _baseUrl;
        private readonly HttpClient _client;

        public StockService()
        {
            this._apiKey = "X0637BX0DJD9U0K6";
            this._baseUrl = "https://www.alphavantage.co/";

            this._client = new HttpClient()
            {
                BaseAddress = new Uri(_baseUrl)
            };
        }
        
        public async Task<Dictionary<string, DailyEntry>> GetTimeSeriesDataAsync(string symbol)
        {
            
            string url = string.Format("query?function=TIME_SERIES_DAILY&symbol={0}&outputsize=compact&apikey={1}",symbol,_apiKey);
            HttpResponseMessage response = await _client.GetAsync(url);

            try
            {
                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    var dailyEntries = JsonSerializer.Deserialize<DailyEntriesCollection>(jsonResponse);

                    if (dailyEntries?.TimeSeriesDaily == null)
                    {
                        throw new Exception("Failed to deserialize API response.");
                    }

                    return dailyEntries.TimeSeriesDaily;
                }
                else
                {
                    throw new Exception($"API request failed with status code: {response.StatusCode}");
                }
            }catch(Exception e)
            {
                throw new Exception("An error occurred while fetching time series data: " + e.Message);
            }
        }

        


    }
}
