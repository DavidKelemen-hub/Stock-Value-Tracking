using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace StockApp.Domain.Models
{
    public class Message
    {
        [JsonPropertyName("title")]
        public string? Title { get; set; }
        [JsonPropertyName("url")]
        public string? Url { get; set; }
        [JsonPropertyName("thumbnail")]
        public string? Thumbnail { get; set; }
    }

    public class NewsFeed
    {
        [JsonPropertyName("newsfeed")] 
        public List<Message> News { get; set; }
    }
}
