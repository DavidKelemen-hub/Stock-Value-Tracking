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

        public MainViewModel()
        {
            processingService = new Processing();
            Companies = new ObservableCollection<Company>(processingService.GetAllCompanies());
            
            RangeClickedCommand = new RelayCommand(param =>
            {
                var range = param as string;
                if (range == null) return;

                chartBuilder = new ChartBuilder(SelectedCompany.Name);
                var prices = processingService.GetStockEntriesBetweenDates(SelectedCompany.Symbol, range);
                ChartData = chartBuilder.LoadChartData(range,prices);
                VariationPercentage = processingService.GetPriceVariation(SelectedCompany.Symbol, range);
            });

        }


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
        public Company SelectedCompany
        {
            get { return selectedCompany; }
            set
            {
                if (selectedCompany == value) return;
                selectedCompany = value;
                OnPropertyChanged();
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

        
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
