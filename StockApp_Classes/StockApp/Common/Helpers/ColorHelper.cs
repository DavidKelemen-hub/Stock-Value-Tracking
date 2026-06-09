using System.Windows.Media;

namespace StockApp.Common.Helpers
{
    public static class ColorHelper
    {
        public static SolidColorBrush GetTrendingColor(bool isPositive)
        {
            return new SolidColorBrush(isPositive ? Colors.LimeGreen : Colors.IndianRed);
        }

        public static SolidColorBrush GetTrendingColor(decimal? priceVariation) =>
            GetTrendingColor(priceVariation > 0);
    }
}
