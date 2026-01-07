using OxyPlot;
using StockApp.Helpers;
using StockApp_Classes.Models;
using StockApp_Classes.Processing;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;


namespace StockApp.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {     
        public ObservableCollection<Company> Companies { get; set; }
        public ICommand RangeClickedCommand { get; set; }
        private Company selectedCompany;
        private PlotModel chartData;
        private readonly Processing processingService;
        private ChartBuilder chartBuilder { get; set; }
        private double variationPercentage { get; set; }
        private double currentPrice { get; set; }
        private string selectedRange { get; set; }

        public MainViewModel(Processing processingService)
        {
            Companies = new ObservableCollection<Company>(processingService.GetAllCompanies());
            this.processingService = processingService;
            this.selectedCompany = Companies.First();
            this.selectedRange = "1Y";
            RefreshData();

            RangeClickedCommand = new RelayCommand(param =>
            {
                SelectedRange = param as string;
                if (SelectedRange == null) return;

                RefreshData();
            });

        }
        /************************************************* Commands ****************************************************/

        private void RefreshData()
        {
            chartBuilder = new ChartBuilder(SelectedCompany.Name);
            var prices = processingService.GetStockEntriesBetweenDates(SelectedCompany.Symbol, SelectedRange);
            ChartData = chartBuilder.LoadChartData(SelectedRange, prices);
            VariationPercentage = processingService.GetPriceVariation(SelectedCompany.Symbol, SelectedRange);
            CurrentPrice = processingService.GetCurrentPrice(SelectedCompany.Symbol);
        }
      
        /************************************************* Commands ****************************************************/

        /*********************************************** Model Properties ****************************************************/
        public double VariationPercentage
        {
            get { return variationPercentage; }
            set
            {
                if (variationPercentage == value) return;
                variationPercentage = value;
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
