using Newtonsoft.Json;
using StockApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace StockApp.Infrastructure.DataAccess
{
    public interface IMessageService
    {
        public Task<NewsFeed> GetNewsFeed(string symbol, int size);
    }
    public class MessageService : IMessageService
    {
        private HttpClient httpClient;
        private string url { get; set; }

        public MessageService()
        {
            this.url = "http://127.0.0.1:8000/";
            httpClient = new HttpClient()
            {
                BaseAddress = new Uri(url)
            };
        }

        public async Task<NewsFeed> GetNewsFeed(string symbol, int size)
        {
            string requestString = $"{this.url}news?symbol={symbol}&size={size}";
            HttpResponseMessage response = await httpClient.GetAsync(requestString);
            NewsFeed myDeserializedClass = new();

            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                myDeserializedClass = JsonSerializer.Deserialize<NewsFeed>(jsonResponse);

            }
            return myDeserializedClass;
        }

       
    }
}
