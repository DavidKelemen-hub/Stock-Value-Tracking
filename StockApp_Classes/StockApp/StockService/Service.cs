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
        private readonly string InitSelectedRange = "1Y";

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

        public Company GetInitialCompany()
        {
            ObservableCollection<Company> _companiesCopy = new ObservableCollection<Company>(_processing.GetAllCompanies());
            return _companiesCopy.FirstOrDefault();
        }

        public string GetInitialRange()
        {
            return InitSelectedRange;
        }

        public List<CompanyPerformance> GetTopPerformers(bool isTop10, string selectedRange)
        {
            return isTop10 ? _processing.GetTopPerformingCompanies(selectedRange) : _processing.GetLowestPerformingCompanies(selectedRange);
        }

        public Brush GetPerformersColor(bool isTop10)
        {
            return isTop10 ? new SolidColorBrush(Colors.LimeGreen) : new SolidColorBrush(Colors.IndianRed);
        }

        public StockDTO LoadData(Company selectedCompany, string selectedRange, bool isTop10, string searchText)
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
            var performers = GetTopPerformers(isTop10, selectedRange);
            var performersColor = GetPerformersColor(isTop10);

            return new StockDTO
            {
                Companies = companies,
                Performers = performers,
                ChartData = chartData,
                PerformersColor = performersColor,
                CurrentPrice = currentPrice,
                PriceVariation = priceVariation,
                PercentageVariation = percentageVariation,
                HighestPrice = highestPrice,
                LowestPrice = lowestPrice,
                RangeText = rangeText

            };
        }
    }
}
