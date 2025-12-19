using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StockApp_Classes.Models;

namespace StockApp_Classes.Processing
{

    public class Processing
    {
        
        public double GetHighestPrice(Dictionary<string, DailyEntry> data)
        {
            return data.Max(x => Convert.ToDouble(x.Value.High));
        }
        
    }
}

