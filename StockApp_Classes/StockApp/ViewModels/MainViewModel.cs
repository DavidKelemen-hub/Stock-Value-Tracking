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
using System.Windows;
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

        /********** Stock Data /**********/
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
        /********** Stock Data **********/

        /********** Financial Statement Data **********/
        private string? _freeCashFlow { get; set; }
        private string? _returnOnEquity { get; set; }
        private string? _debtToEquity { get; set; }
        private string? _grossMargins { get; set; }
        private string? _operatingMargins { get; set; }
        private string? _currentRatio { get; set; }
        private string? _ebitda { get; set; }
        private string? _trailingEPS { get; set; }
        private string? _revenueGrowth { get; set; }
        private string? _netCashPosition { get; set; }

        /********** Financial Statement Data **********/

        private EstimatedFairValues? fairValues { get; set; }

        private readonly CacheItemPolicy policy = new()
        {
            AbsoluteExpiration = DateTimeOffset.Now.AddDays(1)
        };

        private readonly MemoryCache _cache = MemoryCache.Default;

        private readonly IStockService _stockService;
        /************************************************ Bindable properties ****************************************************/


        public MainViewModel(IStockService stockService)
        {

            _stockService = stockService;

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
                FairValues = dto.FairValues;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public async Task LoadFinancialStatementAsync()
        {
            var statement = await _stockService.LoadFinancialStatement(SelectedCompany!);

            FreeCashFlow = statement.FreeCashFlow;
            ReturnOnEquity = statement.ReturnOnEquity;
            DebtToEquity = statement.DebtToEquity;
            GrossMargins = statement.GrossMargins;
            OperatingMargins = statement.OperatingMargins;
            CurrentRatio = statement.CurrentRatio;
            Ebitda = statement.Ebitda;
            TrailingEPS = statement.TrailingEPS;
            RevenueGrowth = statement.RevenueGrowth;
            NetCashPosition = statement.NetCashPosition;
        }

        public async Task Initialize()
        {
            var companiesView = await _stockService.GetAllCompanies();

            CompaniesView = CollectionViewSource.GetDefaultView(companiesView);
            CompaniesView.Filter = CompanyMatches;
            SelectedCompany = companiesView.FirstOrDefault();
            SelectedRange = "1Y";

            await LoadStockDataAsync();
            //await LoadPerformersDataAsync();
            await LoadFinancialStatementAsync();
        }

        private bool CompanyMatches(object obj)
        {
            if (obj is not Company c) return false;

            var s = SearchText?.Trim();
            if (string.IsNullOrEmpty(s)) return true;

            var name = c.Name ?? "";
            var symbol = c.Symbol ?? "";

            return name.Contains(s, StringComparison.OrdinalIgnoreCase)
                || symbol.Contains(s, StringComparison.OrdinalIgnoreCase);
        }

        /*
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
} */
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
                //_ = LoadPerformersDataAsync();
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
                //_ = LoadPerformersDataAsync();
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
                _ = LoadFinancialStatementAsync();
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

        public EstimatedFairValues? FairValues
         {
            get { return fairValues; }
            set
            {
                if (fairValues == value) return;
                fairValues = value;
                OnPropertyChanged();
            }
        }

        public string? FreeCashFlow
        {
            get { return _freeCashFlow; }
            set
            {
                if (_freeCashFlow == value || value == null) return;
                _freeCashFlow = value;
                OnPropertyChanged();
            }
        }

        public string? ReturnOnEquity
        {
            get { return _returnOnEquity; }
            set
            {
                if (_returnOnEquity == value || value == null) return;
                _returnOnEquity = value;
                OnPropertyChanged();
            }
        }

        public string? DebtToEquity
        {
            get { return _debtToEquity; }
            set
            {
                if (_debtToEquity == value || value == null) return;
                _debtToEquity = value;
                OnPropertyChanged();
            }
        }

        public string? GrossMargins
        {
            get { return _grossMargins; }
            set
            {
                if (_grossMargins == value || value == null) return;
                _grossMargins = value;
                OnPropertyChanged();
            }
        }

        public string? OperatingMargins
        {
            get { return _operatingMargins; }
            set
            {
                if (_operatingMargins == value || value == null) return;
                _operatingMargins = value;
                OnPropertyChanged();
            }
        }

        public string? CurrentRatio
        {
            get { return _currentRatio; }
            set
            {
                if (_currentRatio == value || value == null) return;
                _currentRatio = value;
                OnPropertyChanged();
            }
        }

        public string? Ebitda
        {
            get { return _ebitda; }
            set
            {
                if (_ebitda == value || value == null) return;
                _ebitda = value;
                OnPropertyChanged();
            }
        }

        public string? TrailingEPS
        {
            get { return _trailingEPS; }
            set
            {
                if (_trailingEPS == value || value == null) return;
                _trailingEPS = value;
                OnPropertyChanged();
            }
        }

        public string? RevenueGrowth
        {
            get { return _revenueGrowth; }
            set
            {
                if (_revenueGrowth == value || value == null) return;
                _revenueGrowth = value;
                OnPropertyChanged();
            }
        }

        public string? NetCashPosition
        {
            get { return _netCashPosition; }
            set
            {
                if (_netCashPosition == value || value == null) return;
                _netCashPosition = value;
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
