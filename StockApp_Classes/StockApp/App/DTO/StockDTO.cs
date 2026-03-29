using OxyPlot;
using StockApp.Domain.Models;
using System.Collections.ObjectModel;
using System.Windows.Media;

namespace StockApp.Appl.DTO
{
    public class StockDTO
    {
        public ObservableCollection<Company>? Companies { get; set; }
        public Company? SelectedCompany { get; set; }
        public ImageSource? CompanyLogo { get; set; }
        public PlotModel? ChartData { get; set; }
        public Brush? ChartColor { get; set; }
        public double? CurrentPrice { get; set; }
        public double? PriceVariation { get; set; }
        public double? PercentageVariation { get; set; }
        public double? HighestPrice { get; set; }
        public double? LowestPrice { get; set; }
        public string RangeText { get; set; } = String.Empty;
        public string SelectedRange { get; set; } = String.Empty;
        public string SearchText { get; set; } = String.Empty;
        public EstimatedFairValues? FairValues { get; set; }
    }



}
