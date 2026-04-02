using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockApp.Common.Helpers
{
    public static class LargeNumberHelper
    {
        public static string FormatLargeNumber(long value)
        {
            return Math.Abs(value) switch
            {
                >= 1_000_000_000_000 => $"${value / 1_000_000_000_000.0:F2}T",
                >= 1_000_000_000 => $"${value / 1_000_000_000.0:F2}B",
                >= 1_000_000 => $"${value / 1_000_000.0:F2}M",
                >= 1_000 => $"${value / 1_000.0:F2}K",
                _ => $"${value}"
            };
        }
    }
}
