using StockApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockApp.Common.Helpers
{
    public static class NullEntryHelper
    {
        public static void SanitizeInput(List<DailyEntry> dailyEntries)
        {
            dailyEntries.RemoveAll(item =>
                (item.OpenPrice == 0) &&
                (item.LowPrice == 0) &&
                (item.ClosePrice == 0) &&
                (item.HighPrice == 0));
        }
    }
}
