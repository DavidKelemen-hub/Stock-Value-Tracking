namespace StockApp.Domain.Models
{
    public class DailyEntry
    {
        public int StockID { get; set; }
        public DateTime TradeDate { get; set; }
        public double OpenPrice { get; set; }
        public double HighPrice { get; set; }
        public double LowPrice { get; set; }
        public double ClosePrice { get; set; }
        public long Volume { get; set; }
    }
}
