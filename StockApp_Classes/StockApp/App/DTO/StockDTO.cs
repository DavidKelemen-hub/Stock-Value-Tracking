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
        public Chart? ChartData { get; set; }
        public Brush? ChartColor { get; set; }
        public decimal? CurrentPrice { get; set; }
        public decimal? PriceVariation { get; set; }
        public decimal? PercentageVariation { get; set; }
        public decimal? HighestPrice { get; set; }
        public decimal? LowestPrice { get; set; }
        public string RangeText { get; set; } = String.Empty;
        public string SelectedRange { get; set; } = String.Empty;
        public string SearchText { get; set; } = String.Empty;
        public EstimatedFairValues? FairValues { get; set; }
    }



}
