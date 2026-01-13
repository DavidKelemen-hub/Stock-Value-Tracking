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
using StockApp.StockService;
using StockApp.DTOHelper;


namespace StockApp.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {     
        
        /************************************************ Bindable properties ****************************************************/
        public ObservableCollection<Company> companiesCollection { get; set; }
        public ObservableCollection<Company> companiesCopy { get; set; }
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
        public Service _service = new Service();
        public Processing processing = new Processing();
        public SearchHelper searchHelper = new SearchHelper();
        private bool isInitialized = false;
        
        /************************************************ Bindable properties ****************************************************/

        public MainViewModel()
        {
            Companies = new ObservableCollection<Company>(processing.GetAllCompanies());
            companiesCopy = Companies;
            SelectedCompany = _service.GetFilteredCompanies(SearchText).FirstOrDefault();
            SelectedRange = _service.GetInitialRange();


            LoadData();
            isInitialized = true;

            RangeClickedCommand = new RelayCommand(param =>
            {
                SelectedRange = param as string;
                if (SelectedRange == null || SelectedCompany == null) return;

            });
        }
        /************************************************* Commands ****************************************************/
        

        public void LoadData()
        {
            StockDTO dto = _service.LoadData(SelectedCompany, SelectedRange, ShowTop10, SearchText);

            PercentageVariation = dto.PercentageVariation;
            CurrentPrice = dto.CurrentPrice;
            PriceVariation = dto.PriceVariation;
            RangeText = dto.RangeText;
            HighestPrice = dto.HighestPrice;
            LowestPrice = dto.LowestPrice;
            CompaniesPerformance = dto.Performers;
            PerformersColor = dto.PerformersColor;
            //Companies = dto.Companies;
            ChartData = dto.ChartData;
        }

        public void RequestRefresh()
        {
            if (!isInitialized) return;
            LoadData();
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
                RequestRefresh();
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
                RequestRefresh();
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
                RequestRefresh();
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
