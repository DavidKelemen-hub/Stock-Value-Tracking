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
using System.Windows.Data;


namespace StockApp.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {     
        
        /************************************************ Bindable properties ****************************************************/
        public ObservableCollection<Company> companiesCollection { get; set; }
        public ObservableCollection<Company> companiesCopy { get; set; }
        public ICollectionView CompaniesView { get; set; }
        private Company selectedCompany;
        private PlotModel chartData;
        private double percentageVariation { get; set; }
        private double currentPrice { get; set; }
        private string selectedRange { get; set; }
        private string searchText { get; set; }
        private double priceVariation { get; set; }
        private string rangeText { get; set; }
        private string performerRangeText { get; set; }
        private bool showTop10 { get; set; }
        private Brush chartColor { get; set; }
        private Brush performersColor { get; set; }
        public ICommand RangeClickedCommand { get; set; }

        private double highestPrice { get; set; }
        private double lowestPrice { get; set; }
        public List<CompanyPerformance> companiesPerformance { get; set; }
        public Service _service = new Service();
        private bool isInitialized = false;
        private bool _isRefreshing;

        /************************************************ Bindable properties ****************************************************/

        public MainViewModel()
        {
            _ =  InitAsync();

            RangeClickedCommand = new RelayCommand(param =>
            {
                SelectedRange = param as string;
                if (SelectedRange == null || SelectedCompany == null) return;

            });
        }
        /************************************************* Commands ****************************************************/
        private bool CompanyMatches(object obj)
        {
            if (obj is not Company c) return false;

            var s = SearchText?.Trim();
            if (string.IsNullOrEmpty(s)) return true;

            // avoid null issues
            var name = c.Name ?? "";
            var symbol = c.Symbol ?? "";

            return name.Contains(s, StringComparison.OrdinalIgnoreCase)
                || symbol.Contains(s, StringComparison.OrdinalIgnoreCase);
        }


        public async Task LoadData()
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
            ChartData = dto.ChartData;
            ChartColor = dto.ChartColor;
            PerformerRangeText = dto.PerformerRangeText;
        }

        public async Task InitAsync()
        {
            Companies = _service.GetAllCompanies();
            CompaniesView = CollectionViewSource.GetDefaultView(Companies);
            CompaniesView.Filter = CompanyMatches;
            SelectedCompany = _service.GetFilteredCompanies(SearchText).FirstOrDefault();
            SelectedRange = _service.GetInitialRange();

            await LoadData();

            isInitialized = true;
        }

        public async Task  RequestRefresh()
        {
            if (!isInitialized) return;
            await LoadData();
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
                _ = RequestRefresh();
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

        public string PerformerRangeText
        {
            get { return performerRangeText; }
            set
            {
                if (performerRangeText == value) return;
                performerRangeText = value;
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
            get => searchText;
            set
            {
                if (searchText == value) return;
                searchText = value;
                OnPropertyChanged();

                var old = SelectedCompany;

                _isRefreshing = true;
                try { CompaniesView.Refresh(); }
                finally { _isRefreshing = false; }

                // keep selection if still visible
                if (old != null && CompaniesView.Contains(old))
                    SelectedCompany = old;
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
                _ = RequestRefresh();
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

        public Company? SelectedCompany
        {
            get => selectedCompany;
            set
            {
                // ignore transient null pushed by WPF during refresh
                if (value == null && _isRefreshing) return;

                if (ReferenceEquals(selectedCompany, value)) return;
                selectedCompany = value;
                OnPropertyChanged();
                _ = RequestRefresh();
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
