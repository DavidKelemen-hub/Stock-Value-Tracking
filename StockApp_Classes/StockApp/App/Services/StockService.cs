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
        public Task<StockDTO> LoadStockData(Company selectedCompany, string selectedRange);
    }
    public class StockService : IStockService
    {
        private readonly IProcessing _processing;

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

        public async Task<StockDTO> LoadStockData(Company selectedCompany, string selectedRange)
        {
            
            IndividualStockData stockData = await _processing.GetIndividualStockData(selectedCompany.Symbol, selectedRange);

            ChartBuilder chartBuilder = new ChartBuilder(selectedCompany.Name);

            return new StockDTO
            {
                ChartData = chartBuilder.LoadChartData(selectedRange,
                                                       stockData.DailyValues,
                                                       Math.Sign(Convert.ToDouble(stockData.PercentageVariation))),
                CurrentPrice = stockData.CurrentPrice,
                PriceVariation = stockData.PriceVariation,
                PercentageVariation = stockData.PercentageVariation,
                HighestPrice = stockData.HighestPrice,
                LowestPrice = stockData.LowestPrice,
                RangeText = DescriptionHelper.GetRangeDescription(stockData.PriceVariation, selectedRange),
                ChartColor = ColorHelper.GetTrendingColor(stockData.PriceVariation),
            };
        }

        
    }
}
