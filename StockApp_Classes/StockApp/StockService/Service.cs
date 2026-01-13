using Microsoft.IdentityModel.Tokens;
using StockApp.DTOHelper;
using StockApp.Helpers;
using StockApp_Classes.Models;
using StockApp_Classes.Processing;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace StockApp.StockService
{
    public class Service
    {
        private readonly Processing _processing;
        private readonly SearchHelper _searchHelper;

        public Service()
        {
            _processing = new Processing();
            _searchHelper = new SearchHelper();
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
                _companies = new ObservableCollection<Company>(_searchHelper.GetMatchingCompanies(searchText.ToLower(), _companiesCopy));
            }
            return _companies;
        }
        public InitDTO LoadInitData(bool isTop10, string searchText)
        {

            
            var companies = GetFilteredCompanies(searchText);
            var selectedCompany = companies.FirstOrDefault();
            var selectedRange = "1Y";
            var percentageVariation = _processing.GetPercentageVariationInRange(selectedCompany.Symbol, selectedRange);
            var dailyEntries = _processing.GetStockEntriesBetweenDates(selectedCompany.Symbol, selectedRange);
            ChartBuilder chartBuilder = new ChartBuilder(selectedCompany.Name);
            var chartData = chartBuilder.LoadChartData(selectedRange, 
                                                       dailyEntries, 
                                                       Math.Sign(Convert.ToDouble(percentageVariation)));
            var performersColor = isTop10 ? new SolidColorBrush(Colors.LimeGreen) : new SolidColorBrush(Colors.IndianRed);

            return new InitDTO
            {
                Companies = companies,
                SelectedCompany = selectedCompany,
                SelectedRange = selectedRange,
                Performers = isTop10 ? _processing.GetTopPerformingCompanies(selectedRange) : _processing.GetLowestPerformingCompanies(selectedRange),
                ChartData = chartData,
                PerformersColor = performersColor


            };
        }

        public StockDTO LoadData(Company SelectedCompany, string SelectedRange, bool isTop10, string searchText)
        {
            ChartBuilder chartBuilder = new ChartBuilder(SelectedCompany.Name);
            var dailyEntries = _processing.GetStockEntriesBetweenDates(SelectedCompany.Symbol, SelectedRange);
            var percentageVariation = _processing.GetPercentageVariationInRange(SelectedCompany.Symbol, SelectedRange);
            var prices = _processing.GetStockEntriesBetweenDates(SelectedCompany.Symbol, SelectedRange);
            var priceVariation = _processing.GetPriceVariationInRange(SelectedCompany.Symbol, SelectedRange);
            var companies = GetFilteredCompanies(searchText);
            var chartData = chartBuilder.LoadChartData(SelectedRange,
                                                       dailyEntries,
                                                       Math.Sign(Convert.ToDouble(percentageVariation)));

            return new StockDTO
            {
                PercentageVariation = _processing.GetPriceVariationInRange(SelectedCompany.Symbol, SelectedRange),
                CurrentPrice = _processing.GetCurrentPrice(SelectedCompany.Symbol),
                PriceVariation = _processing.GetPriceVariationInRange(SelectedCompany.Symbol, SelectedRange),
                RangeText = _processing.GetRangeDescription(Convert.ToDouble(priceVariation), SelectedRange),
                HighestPrice = _processing.GetHighestPriceInRange(SelectedCompany.Symbol, SelectedRange),
                LowestPrice = _processing.GetLowestPriceInRange(SelectedCompany.Symbol, SelectedRange),
                Performers = isTop10 ? _processing.GetTopPerformingCompanies(SelectedRange) : _processing.GetLowestPerformingCompanies(SelectedRange),
                PerformersColor = isTop10 ? new SolidColorBrush(Colors.LimeGreen) : new SolidColorBrush(Colors.IndianRed),
                Companies = companies,
                ChartData = chartData,
                SelectedCompany = SelectedCompany
            };
        }
    }
}
