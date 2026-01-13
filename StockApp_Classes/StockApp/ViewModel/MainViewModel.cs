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
        private double percentageVariation { get; set; }
        private double currentPrice { get; set; }
        private string selectedRange { get; set; }
        private string searchText { get; set; }
        private double priceVariation { get; set; }
        private string rangeText { get; set; }
        private bool showTop10 { get; set; }
        private Brush chartColor { get; set; }
        private Brush performersColor { get; set; }
        public ICommand RangeClickedCommand { get; set; }
        public ICommand PerformanceSelectorCommand { get; set; }
        private double highestPrice { get; set; }
        private double lowestPrice { get; set; }
        public List<CompanyPerformance> companiesPerformance { get; set; }

        private string topPerformers = "Check Top Performing Companies";
        private string lowPerformers = "Check Lowest Performing Companies";
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
        }
        /************************************************* Commands ****************************************************/
        private void Initialize()
        {
            Companies = new ObservableCollection<Company>(processingService.GetAllCompanies());
            companiesCopy = Companies;
            selectedCompany = companiesCollection.First();
            selectedRange = "1Y";
            LoadPerformingCompanies();
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
            if(ShowTop10 == true)
            {
                CompaniesPerformance = processingService.GetTopPerformingCompanies(SelectedRange);
                PerformersColor = new SolidColorBrush(Colors.LimeGreen);
            }
            else
            {
                CompaniesPerformance = processingService.GetLowestPerformingCompanies(SelectedRange);
                PerformersColor = new SolidColorBrush(Colors.IndianRed);
            }
        }
        private void RefreshData()
        {
            if (SelectedCompany == null) return;
            chartBuilder = new ChartBuilder(SelectedCompany.Name);
            var prices = processingService.GetStockEntriesBetweenDates(SelectedCompany.Symbol, SelectedRange);
            PercentageVariation = processingService.GetPercentageVariationInRange(SelectedCompany.Symbol, SelectedRange);
            CurrentPrice = processingService.GetCurrentPrice(SelectedCompany.Symbol);
            PriceVariation = processingService.GetPriceVariationInRange(SelectedCompany.Symbol, SelectedRange);
            RangeText = processingService.GetRangeDescription(Convert.ToDouble(PriceVariation), SelectedRange);
            HighestPrice = processingService.GetHighestPriceInRange(SelectedCompany.Symbol, SelectedRange);
            LowestPrice = processingService.GetLowestPriceInRange(SelectedCompany.Symbol, SelectedRange);
            ChartData = chartBuilder.LoadChartData(SelectedRange, prices, Math.Sign(Convert.ToDouble(PercentageVariation)));
            

            if (Convert.ToDouble(PriceVariation) < 0)
            {
                ChartColor = new SolidColorBrush(Colors.IndianRed);
            }
            else
            {
                ChartColor = new SolidColorBrush(Colors.LimeGreen);
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

        public bool ShowTop10
        {
            get { return showTop10; }
            set
            {
                if (showTop10 == value) return;
                showTop10 = value;
                OnPropertyChanged();
                LoadPerformingCompanies();
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

        public Brush ChartColor
        {
            get { return chartColor; }
            set
            {
                if (chartColor == value) return;
                chartColor = value;
                OnPropertyChanged();
            }
        }

        public Brush PerformersColor
        {
            get { return performersColor; }
            set
            {
                if (performersColor == value) return;
                performersColor = value;
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
        public double PriceVariation
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
            public double PercentageVariation
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
                LoadPerformingCompanies();
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
