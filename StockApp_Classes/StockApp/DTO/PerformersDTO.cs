using StockApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace StockApp.DTO
{
    public class PerformersDTO
    {
        public List<CompanyPerformance> Performers { get; set; }
        public Brush PerformersColor { get; set; }
        public string PerformerRangeText { get; set; }
        public bool IsTop10 { get; set; }
    }
}
