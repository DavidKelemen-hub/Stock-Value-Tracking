using Microsoft.IdentityModel.Tokens;
using StockApp.Appl.DTO;
using StockApp.Common.Helpers;
using StockApp.Domain.Models;
using StockApp.Domain.Processing;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace StockApp.Appl.Services
{
    public interface IStockService
    {
        public Task<ObservableCollection<Company>> GetAllCompanies();
        public Task<ObservableCollection<Company>> GetFilteredCompanies(string searchText);
        public Task<StockDTO> LoadStockData(Company selectedCompany, string selectedRange);
    }
    public class StockService : IStockService
    {
        private readonly IProcessing _processing;

        public StockService(IProcessing _processing)
        {
            this._processing = _processing;
        }

        public async Task<ObservableCollection<Company>> GetAllCompanies()
        {
            var result = await _processing.GetAllCompanies().ConfigureAwait(false);
            return new ObservableCollection<Company>(result);
        }

        public async Task<ObservableCollection<Company>> GetFilteredCompanies(string searchText)
        {
            ObservableCollection<Company> _companies;
            var result = await _processing.GetAllCompanies().ConfigureAwait(false);
            ObservableCollection<Company> _companiesCopy = new(result);

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
            var stockTask = _processing.GetIndividualStockData(selectedCompany.Symbol!, selectedRange);
            var fairValuesTask = _processing.GetEstimatedFairValues(selectedCompany.Symbol!);

            await Task.WhenAll(stockTask, fairValuesTask);

            IndividualStockData stockData = stockTask.Result;
            EstimatedFairValues fairValues = fairValuesTask.Result;
            ChartBuilder chartBuilder = new(selectedCompany.Name!);

            return new StockDTO
            {
                ChartData = chartBuilder.LoadChartData(selectedRange,
                                                       stockData.DailyValues!,
                                                       Math.Sign(Convert.ToDouble(stockData.PercentageVariation))),
                CurrentPrice = stockData.CurrentPrice,
                PriceVariation = stockData.PriceVariation,
                PercentageVariation = stockData.PercentageVariation,
                HighestPrice = stockData.HighestPrice,
                LowestPrice = stockData.LowestPrice,
                RangeText = DescriptionHelper.GetRangeDescription(stockData.PriceVariation, selectedRange),
                ChartColor = ColorHelper.GetTrendingColor(stockData.PriceVariation),
                CompanyLogo = LogoHelper.GetCompanyLogo(selectedCompany.Symbol!),
                FairValues = fairValues
            };
        }
    }
}
