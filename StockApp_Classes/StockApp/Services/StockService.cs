using Microsoft.IdentityModel.Tokens;
using StockApp.DTO;
using StockApp.Helpers;
using StockApp.Models;
using StockApp.ProcessingService;
using System.Collections.ObjectModel;
using System.Windows.Media;

namespace StockApp.Services
{
    public interface IStockService
    {
        public ObservableCollection<Company> GetAllCompanies();
        public ObservableCollection<Company> GetFilteredCompanies(string searchText);
        public string GetInitialRange();
        public StockDTO LoadStockData(Company selectedCompany, string selectedRange, bool isTop10, string searchText);
    }
    public class StockService : IStockService
    {
        private readonly IProcessing _processing;
        private readonly string InitSelectedRange = "1Y";

        public StockService(IProcessing _processing)
        {
            this._processing = _processing;
        }

        public ObservableCollection<Company> GetAllCompanies()
        {
            return new ObservableCollection<Company>(_processing.GetAllCompanies());
        }

        public ObservableCollection<Company> GetFilteredCompanies(string searchText)
        {
            ObservableCollection<Company> _companies;
            ObservableCollection<Company> _companiesCopy = new ObservableCollection<Company>(_processing.GetAllCompanies());

            if (searchText.IsNullOrEmpty())
            {
                _companies = _companiesCopy;
            }
            else
            {
                _companies = new ObservableCollection<Company>(SearchHelper.GetMatchingCompanies(searchText.ToLower(), _companiesCopy));
            }
            return _companies;
        }

        public string GetInitialRange()
        {
            return InitSelectedRange;
        }

        public StockDTO LoadStockData(Company selectedCompany, string selectedRange, bool isTop10, string searchText)
        {
            
            ChartBuilder chartBuilder = new ChartBuilder(selectedCompany.Name);
            var dailyEntries = _processing.GetStockEntriesBetweenDates(selectedCompany.Symbol, selectedRange);
            var percentageVariation = _processing.GetPercentageVariationInRange(selectedCompany.Symbol, selectedRange);
            var prices = _processing.GetStockEntriesBetweenDates(selectedCompany.Symbol, selectedRange);
            var priceVariation = _processing.GetPriceVariationInRange(selectedCompany.Symbol, selectedRange);
            var companies = GetFilteredCompanies(searchText);
            var chartData = chartBuilder.LoadChartData(selectedRange,
                                                       dailyEntries,
                                                       Math.Sign(Convert.ToDouble(percentageVariation)));

            var currentPrice = _processing.GetCurrentPrice(selectedCompany.Symbol);
            var highestPrice = _processing.GetHighestPriceInRange(selectedCompany.Symbol, selectedRange);
            var lowestPrice = _processing.GetLowestPriceInRange(selectedCompany.Symbol, selectedRange);
            var rangeText = _processing.GetRangeDescription(Convert.ToDouble(priceVariation), selectedRange);
            var chartColor = priceVariation > 0 ? new SolidColorBrush(Colors.LimeGreen) : new SolidColorBrush(Colors.IndianRed);

            return new StockDTO
            {
                Companies = companies,
                ChartData = chartData,
                CurrentPrice = currentPrice,
                PriceVariation = priceVariation,
                PercentageVariation = percentageVariation,
                HighestPrice = highestPrice,
                LowestPrice = lowestPrice,
                RangeText = rangeText,
                ChartColor = chartColor,
            };
        }

        
    }
}
