using Microsoft.Data.SqlClient;
using OxyPlot;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Axes;
using OxyPlot.Series;
using StockApp.Helpers;
using StockApp_Classes.Models;
using StockApp_Classes.Processing;
using StockApp_Classes.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;


namespace StockApp.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {     
        public ObservableCollection<Company> Companies { get; set; }
        public ICommand RangeClickedCommand { get; set; }
        private Company selectedCompany;
        public PlotModel chartData;
        private Processing processingService { get; set; }
        public ChartBuilder chartBuilder { get; set; }
        public double variationPercengage { get; set; }
        public double currentPrice { get; set; }
        public string selectedRange { get; set; }

        public MainViewModel()
        {
            processingService = new Processing();
            Companies = new ObservableCollection<Company>(processingService.GetAllCompanies());

            HandleInitialChartLoad();
            HandleRangeClickedCommand();

        }

        /************************************************* Commands ****************************************************/
        private void HandleRangeClickedCommand()
        {
            RangeClickedCommand = new RelayCommand(param =>
            {
                SelectedRange = param as string;
                if (SelectedRange == null) return;

                chartBuilder = new ChartBuilder(SelectedCompany.Name);
                var prices = processingService.GetStockEntriesBetweenDates(SelectedCompany.Symbol, SelectedRange);
                ChartData = chartBuilder.LoadChartData(SelectedRange, prices);
                VariationPercentage = processingService.GetPriceVariation(SelectedCompany.Symbol, SelectedRange);
                CurrentPrice = processingService.GetCurrentPrice(SelectedCompany.Symbol);
            });
        }

        private void HandleSelectionChanged()
        {
            selectedRange = "1Y";
            chartBuilder = new ChartBuilder(SelectedCompany.Name);
            var prices = processingService.GetStockEntriesBetweenDates(SelectedCompany.Symbol, SelectedRange);
            ChartData = chartBuilder.LoadChartData(SelectedRange, prices);
            VariationPercentage = processingService.GetPriceVariation(SelectedCompany.Symbol, SelectedRange);
            CurrentPrice = processingService.GetCurrentPrice(SelectedCompany.Symbol);
        }

        public void HandleInitialChartLoad()
        {
            /* At startup we select the first company inorder to not have a blank screen */
            selectedCompany = Companies.First();
            selectedRange = "1Y";
            chartBuilder = new ChartBuilder(SelectedCompany.Name);

            /* Calculate data and load chart with the entries */
            var prices = processingService.GetStockEntriesBetweenDates(SelectedCompany.Symbol, selectedRange);
            ChartData = chartBuilder.LoadChartData(selectedRange, prices);
            VariationPercentage = processingService.GetPriceVariation(SelectedCompany.Symbol, selectedRange);
            CurrentPrice = processingService.GetCurrentPrice(SelectedCompany.Symbol);
        }


        /************************************************* Commands ****************************************************/

        /*********************************************** Model Properties ****************************************************/
        public double VariationPercentage
        {
            get { return variationPercengage; }
            set
            {
                if (variationPercengage == value) return;
                variationPercengage = value;
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
                HandleSelectionChanged();
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
