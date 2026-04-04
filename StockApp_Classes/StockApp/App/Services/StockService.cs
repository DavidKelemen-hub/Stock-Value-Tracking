using Microsoft.IdentityModel.Tokens;
using StockApp.Common.Helpers;
using StockApp.Domain.Models;
using StockApp.Domain.Processing;
using System.Collections.ObjectModel;
using StockApp.Appl.DTO;

namespace StockApp.Appl.Services
{
    public interface IStockService
    {
        public Task<ObservableCollection<Company>> GetAllCompanies();
        public Task<ObservableCollection<Company>> GetFilteredCompanies(string searchText);
        public Task<StockDTO> LoadStockData(Company selectedCompany, string selectedRange);
        public Task<FinancialStatementDTO> LoadFinancialStatement(Company selectedCompany);
        public Task<NewsFeed> GetNewsFeed(string symbol, int size);
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

        public async Task<FinancialStatementDTO?> LoadFinancialStatement(Company selectedCompany)
        {
            var statement = await _processing.GetFinancialStatement(selectedCompany.Symbol!).ConfigureAwait(false);

            if (statement == null) return new FinancialStatementDTO { };

            return new FinancialStatementDTO
            {
                // Valuation & Per Share
                TrailingEPS = statement.TrailingEPS.HasValue ? $"${statement.TrailingEPS.Value:F2}" : "N/A",
                ForwardEPS = statement.ForwardEPS.HasValue ? $"${statement.ForwardEPS.Value:F2}" : "N/A",
                BookValue = statement.BookValue.HasValue ? $"${statement.BookValue.Value:F2}" : "N/A",

                // Profitability & Margins
                GrossMargins = statement.GrossMargins.HasValue ? $"{statement.GrossMargins.Value:P1}" : "N/A",
                OperatingMargins = statement.OperatingMargins.HasValue ? $"{statement.OperatingMargins.Value:P1}" : "N/A",
                Ebitda = statement.EBITDA.HasValue ? LargeNumberHelper.FormatLargeNumber(statement.EBITDA.Value) : "N/A",

                // Growth
                RevenueGrowth = statement.RevenueGrowth.HasValue ? $"{(statement.RevenueGrowth.Value > 0 ? "+" : "")}{statement.RevenueGrowth.Value:P1}" : "N/A",
                EarningsGrowth = statement.EarningsGrowth.HasValue ? $"{(statement.EarningsGrowth.Value > 0 ? "+" : "")}{statement.EarningsGrowth.Value:P1}" : "N/A",

                // Financial Health
                TotalDebt = statement.TotalDebt.HasValue ? LargeNumberHelper.FormatLargeNumber(statement.TotalDebt.Value) : "N/A",
                NetCashPosition = statement.TotalCash.HasValue &&
                                    statement.TotalDebt.HasValue ? LargeNumberHelper.FormatLargeNumber(statement.TotalCash.Value - statement.TotalDebt.Value) : "N/A",
                FreeCashFlow = statement.FreeCashFlow.HasValue ? LargeNumberHelper.FormatLargeNumber(statement.FreeCashFlow.Value) : "N/A",
                CurrentRatio = statement.CurrentRatio.HasValue ? $"{statement.CurrentRatio.Value:F2}x" : "N/A",

                // Dividends
                DividendRate = statement.DividendRate.HasValue ? $"${statement.DividendRate.Value:F2}" : "N/A",
                DividendYield = statement.DividendYield.HasValue ? $"{statement.DividendYield.Value:F2}%" : "N/A",

                // Market & Risk
                Beta = statement.Beta.HasValue ? $"{statement.Beta.Value:F2}" : "N/A",
                SharesOutstanding = statement.SharesOutstanding.HasValue ? LargeNumberHelper.FormatLargeNumber(statement.SharesOutstanding.Value) : "N/A",
            };
        }

        public async Task<NewsFeed> GetNewsFeed(string symbol, int size)
        {
            return await _processing.GetNewsFeed(symbol,size).ConfigureAwait(false);
        }
    }
}
