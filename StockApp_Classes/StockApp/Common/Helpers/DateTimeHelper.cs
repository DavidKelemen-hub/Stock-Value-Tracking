namespace StockApp.Common.Helpers
{
    public static class DateTimeHelper
    {
        
        public static DateTime GetStartDate(string range)
        {
            DateTime startDate = DateTime.Today;
            switch (range)
            {
                case "5D":
                    startDate = DateTime.Today.AddDays(-5);
                    break;
                case "1M":
                    startDate = DateTime.Today.AddMonths(-1);
                    break;
                case "3M":
                    startDate = DateTime.Today.AddMonths(-3);
                    break;
                case "6M":
                    startDate = DateTime.Today.AddMonths(-6);
                    break;
                case "1Y":
                    startDate = DateTime.Today.AddYears(-1);
                    break;
                case "YTD":
                    startDate = new DateTime(DateTime.Today.Year, 1, 1);
                    break;
                case "5Y":
                    startDate = DateTime.Today.AddYears(-5);
                    break;
                default:
                    startDate = DateTime.Today.AddYears(-5);
                    break;
            }
            return startDate;
        }

    }
}
