using OxyPlot;
using StockApp.Appl.DTO;
using StockApp.Appl.Services;
using StockApp.Common.Helpers;
using StockApp.Domain.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Resources;
using System.Runtime.Caching;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace StockApp.ViewModels
{
    public interface IMainViewModel
    {

    }
    public class MainViewModel : INotifyPropertyChanged, IMainViewModel
    {

        /************************************************ Bindable properties ****************************************************/
        public ObservableCollection<Company>? companiesCollection { get; set; }
        public ICollectionView? companiesView { get; set; }
        private Company? selectedCompany;
        private PlotModel? chartData;
        private double? percentageVariation { get; set; }
        private double? currentPrice { get; set; }
        private string? selectedRange { get; set; }
        private string? searchText { get; set; }
        private double? priceVariation { get; set; }
        private string? rangeText { get; set; }
        private string? performerRangeText { get; set; }
        private bool showTop10 { get; set; }
        private Brush? chartColor { get; set; }
        private Brush? performersColor { get; set; }
        public ICommand RangeClickedCommand { get; set; }
        private double? highestPrice { get; set; }
        private double? lowestPrice { get; set; }
        public List<CompanyPerformance>? companiesPerformance { get; set; }
        private ImageSource? companyLogo { get; set; }

        private readonly CacheItemPolicy policy = new()
        {
            AbsoluteExpiration = DateTimeOffset.Now.AddDays(1)
        };

        private readonly MemoryCache _cache = MemoryCache.Default;

        private readonly IStockService _stockService;
        private readonly IPerformersService _performersService;
        /************************************************ Bindable properties ****************************************************/


        public MainViewModel(IStockService stockService, IPerformersService performersService)
        {

            _stockService = stockService;
            _performersService = performersService;

            _ = Initialize();

            RangeClickedCommand = new RelayCommand(param =>
            {
                SelectedRange = param as string;
                if (SelectedRange == null || SelectedCompany == null) return;

            });
        }
        /************************************************* Commands ****************************************************/
        public async Task LoadStockDataAsync()
        {
            if (SelectedCompany == null) return;

            try
            {
                string cacheKey = $"{SelectedCompany.Name}_{SelectedRange}";
                StockDTO dto;

                if (_cache[cacheKey] is StockDTO cached)
                {
                    dto = cached;
                }
                else
                {
                    dto = await _stockService.LoadStockData(SelectedCompany, SelectedRange!);
                    _cache.Set(cacheKey, dto, policy);
                }

                PercentageVariation = dto.PercentageVariation;
                CurrentPrice = dto.CurrentPrice;
                PriceVariation = dto.PriceVariation;
                RangeText = dto.RangeText;
                HighestPrice = dto.HighestPrice;
                LowestPrice = dto.LowestPrice;
                ChartData = dto.ChartData!;
                ChartColor = dto.ChartColor!;
                CompanyLogo = dto.CompanyLogo;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public async Task LoadPerformersDataAsync()
        {
            string cacheKey = $"{ShowTop10}_{SelectedRange}";
            PerformersDTO dto;

            if (_cache[cacheKey] is PerformersDTO cached)
            {
                dto = cached;
            }
            else
            {
                dto = await _performersService.LoadPerformersData(ShowTop10, SelectedRange!);
                _cache.Set(cacheKey, dto, policy);
            }

            CompaniesPerformance = dto.Performers;
            PerformersColor = dto.PerformersColor;
            PerformerRangeText = dto.PerformerRangeText;
        }

        public async Task Initialize()
        {
            var companiesView = await _stockService.GetAllCompanies();

            CompaniesView = CollectionViewSource.GetDefaultView(companiesView);
            CompaniesView.Filter = CompanyMatches;
            SelectedCompany = companiesView.FirstOrDefault();
            SelectedRange = "1Y";

            await LoadStockDataAsync();
            await LoadPerformersDataAsync();
        }

        private bool CompanyMatches(object obj)
        {
            if (obj is not Company c) return false;

            var s = SearchText?.Trim();
            if (string.IsNullOrEmpty(s)) return true;

            return c.Name!.Contains(s)
                || c.Symbol!.Contains(s);
        }
        /************************************************* Commands ****************************************************/

        /*********************************************** Model Properties ****************************************************/
        public ObservableCollection<Company>? Companies
        {
            get { return companiesCollection; }
            set
            {
                if (companiesCollection == value) return;
                companiesCollection = value;
                OnPropertyChanged();
            }
        }
        public List<CompanyPerformance>? CompaniesPerformance
        {
            get { return companiesPerformance; }
            set
            {
                if (companiesPerformance == value) return;
                companiesPerformance = value;
                OnPropertyChanged();
            }
        }

        public bool ShowTop10
        {
            get { return showTop10; }
            set
            {
                if (showTop10 == value) return;
                showTop10 = value;
                OnPropertyChanged();
                _ = LoadPerformersDataAsync();
            }
        }

        public double? HighestPrice
        {
            get { return highestPrice; }
            set
            {
                if (highestPrice == value) return;
                highestPrice = value;
                OnPropertyChanged();
            }
        }

        public double? LowestPrice
        {
            get { return lowestPrice; }
            set
            {
                if (lowestPrice == value) return;
                lowestPrice = value;
                OnPropertyChanged();
            }
        }

        public Brush? ChartColor
        {
            get { return chartColor; }
            set
            {
                if (chartColor == value) return;
                chartColor = value;
                OnPropertyChanged();
            }
        }

        public Brush? PerformersColor
        {
            get { return performersColor; }
            set
            {
                if (performersColor == value) return;
                performersColor = value;
                OnPropertyChanged();
            }
        }
        public string? RangeText
        {
            get { return rangeText; }
            set
            {
                if (rangeText == value) return;
                rangeText = value;
                OnPropertyChanged();
            }
        }

        public string? PerformerRangeText
        {
            get { return performerRangeText; }
            set
            {
                if (performerRangeText == value) return;
                performerRangeText = value;
                OnPropertyChanged();
            }
        }
        public double? PriceVariation
        {
            get { return priceVariation; }
            set
            {
                if (priceVariation == value) return;
                priceVariation = value;
                OnPropertyChanged();
            }
        }
        public string? SearchText
        {
            get => searchText;
            set
            {
                if (searchText == value) return;
                searchText = value;
                OnPropertyChanged();
                CompaniesView!.Refresh();
            }
        }
        public double? PercentageVariation
        {
            get { return percentageVariation; }
            set
            {
                if (percentageVariation == value) return;
                percentageVariation = value;
                OnPropertyChanged();
            }
        }

        public string? SelectedRange
        {
            get { return selectedRange; }
            set
            {
                if (selectedRange == value) return;
                selectedRange = value;
                OnPropertyChanged();
                _ = LoadStockDataAsync();
                _ = LoadPerformersDataAsync();
            }
        }

        public double? CurrentPrice
        {
            get { return currentPrice; }
            set
            {
                if (currentPrice == value) return;
                currentPrice = value;
                OnPropertyChanged();
            }
        }

        public Company? SelectedCompany
        {
            get => selectedCompany;
            set
            {
                if (value == null || selectedCompany == value) return;

                selectedCompany = value;
                OnPropertyChanged();
                _ = LoadStockDataAsync();
            }
        }

        public PlotModel? ChartData
        {
            get { return chartData; }
            set
            {
                if (chartData == value) return;
                chartData = value;
                OnPropertyChanged();
            }
        }

        public ImageSource? CompanyLogo
        {
            get { return companyLogo; }
            set
            {
                if (companyLogo == value) return;
                companyLogo = value;
                OnPropertyChanged();
            }
        }

        public ICollectionView? CompaniesView
        {
            get { return companiesView; }
            set
            {
                if (companiesView == value) return;
                companiesView = value;
                OnPropertyChanged();
            }
        }
        /*********************************************** Model Properties ****************************************************/

        /*********************************************** INotifyPropertyChanged Implementation ****************************************************/
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        /*********************************************** INotifyPropertyChanged Implementation ****************************************************/

    }
}
