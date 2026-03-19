using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockApp.Common.Helpers
{
    public static class DescriptionHelper
    {
        public static string GetRangeDescription(double priceVariation, string range)
        {
            string description = range switch
            {
                "5D" => "past 5 days",
                "1M" => "past month",
                "3M" => "past 3 months",
                "6M" => "past 6 months",
                "YTD" => "year to date",
                "1Y" => "past year",
                "5Y" => "past 5 years",
                "Max" => "all time"
            };

            string trendArrow = priceVariation >= 0 ? "▲ " : "▼ ";

            return string.Concat(trendArrow, description);
        }
    }
}
