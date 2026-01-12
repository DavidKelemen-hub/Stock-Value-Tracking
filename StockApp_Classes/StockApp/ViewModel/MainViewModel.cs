using Microsoft.IdentityModel.Tokens;
using OxyPlot;
using StockApp.Helpers;
using StockApp_Classes.Models;
using StockApp_Classes.Processing;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows.Media;


namespace StockApp.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {     
        
        private ObservableCollection<Company> companiesCopy { get; set; }
        private readonly Processing processingService;
        private ChartBuilder chartBuilder { get; set; }
        private SearchHelper searchHelper = new SearchHelper();
        /************************************************ Bindable properties ****************************************************/
        public ObservableCollection<Company> companiesCollection { get; set; }
        private Company selectedCompany;
        private PlotModel chartData;
        private string percentageVariation { get; set; }
        private double currentPrice { get; set; }
        private string selectedRange { get; set; }
        private string searchText { get; set; }
        private string priceVariation { get; set; }
        private string rangeText { get; set; }
        private string performanceSelector { get; set; }
        private Brush textColor { get; set; }
        public ICommand RangeClickedCommand { get; set; }
        public ICommand PerformanceSelectorCommand { get; set; }
        private double highestPrice { get; set; }
        private double lowestPrice { get; set; }
        public List<CompanyPerformance> companiesPerformance { get; set; }
        /************************************************ Bindable properties ****************************************************/

        public MainViewModel(Processing service)
        {
            this.processingService = service;

            Initialize();
            LoadMatchingCompanies();
            RefreshData();

            RangeClickedCommand = new RelayCommand(param =>
            {
                SelectedRange = param as string;
                if (SelectedRange == null || SelectedCompany == null) return;

                RefreshData();
            });

            PerformanceSelectorCommand = new RelayCommand(param =>
            {
                var selection = param as string;

                LoadPerformingCompanies();
            });
        }
        /************************************************* Commands ****************************************************/
        private void Initialize()
        {
            Companies = new ObservableCollection<Company>(processingService.GetAllCompanies());
            companiesCopy = Companies;
            selectedCompany = companiesCollection.First();
            selectedRange = "1Y";
            PerformanceSelector = new String("Check Lowest Performing Companies");   
        }
        private void LoadMatchingCompanies()
        {
            if (SearchText.IsNullOrEmpty())
            {
                Companies = companiesCopy;
            }
            else
            {
                Companies = new ObservableCollection<Company>(searchHelper.GetMatchingCompanies(SearchText.ToLower(), companiesCopy));
            }
        }

        private void LoadPerformingCompanies()
        {
            if(PerformanceSelector == "Check Top Performing Companies")
            {
                CompaniesPerformance = processingService.GetTopPerformingCompanies(SelectedRange);
                PerformanceSelector = "Check Lowest Performing Companies";
            }
            else
            {
                CompaniesPerformance = processingService.GetLowestPerformingCompanies(SelectedRange);
                PerformanceSelector = "Check Top Performing Companies";
            }
        }
        private void RefreshData()
        {
            if (SelectedCompany == null) return;
            chartBuilder = new ChartBuilder(SelectedCompany.Name);
            var prices = processingService.GetStockEntriesBetweenDates(SelectedCompany.Symbol, SelectedRange);
            PercentageVariation = processingService.GetPercentageVariationInRange(SelectedCompany.Symbol, SelectedRange).ToString();
            CurrentPrice = processingService.GetCurrentPrice(SelectedCompany.Symbol);
            PriceVariation = processingService.GetPriceVariationInRange(SelectedCompany.Symbol, SelectedRange).ToString();
            RangeText = processingService.GetRangeDescription(Convert.ToDouble(PriceVariation), SelectedRange);
            HighestPrice = processingService.GetHighestPriceInRange(SelectedCompany.Symbol, SelectedRange);
            LowestPrice = processingService.GetLowestPriceInRange(SelectedCompany.Symbol, SelectedRange);
            ChartData = chartBuilder.LoadChartData(SelectedRange, prices, Math.Sign(Convert.ToDouble(PercentageVariation)));
            CompaniesPerformance = processingService.GetTopPerformingCompanies(SelectedRange);

            if (Convert.ToDouble(PriceVariation) < 0)
            {
                TextColor = Brushes.Red;
            }
            else
            {
                TextColor = Brushes.Green;
            }
        }

        private List<CompanyPerformance>GetPerformers(string option)
        {
            if(option == "")
            {
                
                return processingService.GetTopPerformingCompanies(SelectedRange);
            }
            else
            {
                return processingService.GetLowestPerformingCompanies(SelectedRange);
            }
        }
        /************************************************* Commands ****************************************************/

        /*********************************************** Model Properties ****************************************************/
        public ObservableCollection<Company> Companies
        {
            get { return companiesCollection; }
            set
            {
                if (companiesCollection == value) return;
                companiesCollection = value;
                OnPropertyChanged();
            }
        }

        public List<CompanyPerformance> CompaniesPerformance
        {
            get { return companiesPerformance; }
            set
            {
                if (companiesPerformance == value) return;
                companiesPerformance = value;
                OnPropertyChanged();
            }
        }

        public string PerformanceSelector
        {
            get { return performanceSelector; }
            set
            {
                if (performanceSelector == value) return;
                performanceSelector = value;
                OnPropertyChanged();
            }
        }

        public double HighestPrice
        {
            get { return highestPrice; }
            set
            {
                if (highestPrice == value) return;
                highestPrice = value;
                OnPropertyChanged();
            }
        }

        public double LowestPrice
        {
            get { return lowestPrice; }
            set
            {
                if (lowestPrice == value) return;
                lowestPrice = value;
                OnPropertyChanged();
            }
        }

        public Brush TextColor
        {
            get { return textColor; }
            set
            {
                if (textColor == value) return;
                textColor = value;
                OnPropertyChanged();
            }
        }
        public string RangeText
        {
            get { return rangeText; }
            set
            {
                if (rangeText == value) return;
                rangeText = value;
                OnPropertyChanged();
            }
        }
        public string PriceVariation
        {
            get { return priceVariation; }
            set
            {
                if (priceVariation == value) return;
                priceVariation = value;
                OnPropertyChanged();
            }
        }
        public string SearchText
        {
            get { return searchText; }
            set
            {
                if (searchText == value) return;
                searchText = value;
                OnPropertyChanged();
                LoadMatchingCompanies();
            }
        }
            public string PercentageVariation
        {
            get { return percentageVariation; }
            set
            {
                if (percentageVariation == value) return;
                percentageVariation = value;
                OnPropertyChanged();
            }
        }

        public string SelectedRange
        {
            get { return selectedRange; }
            set
            {
                if (selectedRange == value) return;
                selectedRange = value;
                OnPropertyChanged();
            }
        }

        public double CurrentPrice
        {
            get { return currentPrice; }
            set
            {
                if (currentPrice == value) return;
                currentPrice = value;
                OnPropertyChanged();
            }
        }

        public Company SelectedCompany
        {
            get { return selectedCompany; }
            set
            {
                if (selectedCompany == value) return;
                selectedCompany = value;
                OnPropertyChanged();
                RefreshData();
            }
        }

        public PlotModel ChartData
        {
            get { return chartData; }
            set
            {
                if (chartData == value) return;
                chartData = value;
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
