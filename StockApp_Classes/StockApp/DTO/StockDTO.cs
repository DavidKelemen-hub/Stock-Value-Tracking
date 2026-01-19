using OxyPlot;
using StockApp.Models;
using System.Collections.ObjectModel;
using System.Windows.Media;

namespace StockApp.DTO
{
    public class StockDTO
    {
        public ObservableCollection<Company> Companies { get; set; } 
        public Company SelectedCompany { get; set; }
        public PlotModel ChartData { get; set; }
        public Brush ChartColor { get; set; }
        public double CurrentPrice { get; set; }
        public double PriceVariation { get; set; }
        public double PercentageVariation { get; set; }
        public double HighestPrice { get; set; }
        public double LowestPrice { get; set; }
        public string RangeText { get; set; }
        public String SelectedRange { get; set; }
        public string SearchText { get; set; }
    }

    

}
