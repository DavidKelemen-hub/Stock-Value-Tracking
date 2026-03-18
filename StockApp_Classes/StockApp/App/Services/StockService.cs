using Microsoft.IdentityModel.Tokens;
using StockApp.Appl.DTO;
using StockApp.Common.Helpers;
using StockApp.Domain.Models;
using StockApp.Domain.Processing;
using System.Collections.ObjectModel;
using System.Windows.Media;

namespace StockApp.Appl.Services
{
    public interface IStockService
    {
        public ObservableCollection<Company> GetAllCompanies();
        public ObservableCollection<Company> GetFilteredCompanies(string searchText);
        public string GetInitialRange();
        public StockDTO LoadStockData(Company selectedCompany, string selectedRange);
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

        public StockDTO LoadStockData(Company selectedCompany, string selectedRange)
        {
            ChartBuilder chartBuilder = new ChartBuilder(selectedCompany.Name);
            IndividualStockData stockData = _processing.GetIndividualStockData(selectedCompany.Symbol, selectedRange);

            var dailyEntries = stockData.DailyValues;
            var percentageVariation = stockData.PercentageVariation;
            var priceVariation = stockData.PriceVariation;
            var currentPrice = stockData.CurrentPrice;
            var lowestPrice = stockData.LowestPrice;
            var highestPrice = stockData.HighestPrice;

            var chartData = chartBuilder.LoadChartData(selectedRange,
                                                       dailyEntries,
                                                       Math.Sign(Convert.ToDouble(percentageVariation)));

            var rangeText = DescriptionHelper.GetRangeDescription(priceVariation, selectedRange);
            var chartColor = ColorHelper.GetTrendingColor(priceVariation);

            return new StockDTO
            {
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
