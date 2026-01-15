using OxyPlot;
using StockApp_Classes.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace StockApp.DTOHelper
{
   

    public class StockDTO
    {
        public ObservableCollection<Company> Companies { get; set; } 
        public Company SelectedCompany { get; set; }
        public List<CompanyPerformance> Performers { get; set; }
        public PlotModel ChartData { get; set; }
        public Brush PerformersColor { get; set; }
        public Brush ChartColor { get; set; }
        public double CurrentPrice { get; set; }
        public double PriceVariation { get; set; }
        public double PercentageVariation { get; set; }
        public double HighestPrice { get; set; }
        public double LowestPrice { get; set; }
        public string RangeText { get; set; }
        public string PerformerRangeText { get; set; }
        public String SelectedRange { get; set; }
        public bool IsTop10 { get; set; }
        public string SearchText { get; set; }
    }

}
