using OxyPlot;
using OxyPlot.Axes;
using StockApp_Classes.Models;
using StockApp_Classes.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System.Windows.Input;
using Microsoft.Data.SqlClient;
using StockApp.Helpers;


namespace StockApp.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {     
        public ObservableCollection<Company> Companies { get; set; }
        public ICommand RangeClickedCommand { get; set; }
        private Company selectedCompany;
        private PlotModel chartData;
        private DataBaseService dbService { get; set; }

        public MainViewModel()
        {
            dbService = new DataBaseService(System.Configuration.ConfigurationManager.ConnectionStrings["dbconnection"].ConnectionString);
            Companies = new ObservableCollection<Company>(dbService.GetAllCompanies());
            RangeClickedCommand = new RelayCommand(param =>
            {
                var range = param as string;
                if (range == null) return;

                LoadChartData(range);
            });

        }

        public Company SelectedCompany
        {
            get { return selectedCompany; }
            set
            {
                if (selectedCompany == value) return;
                selectedCompany = value;
                //LoadChartData();
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

        private void LoadChartData(string range)
        {
            if (SelectedCompany == null)
                return;

            List<DailyEntry> prices;
            if(range == "5D")
            {
               prices = dbService.GetStockEntriesBetweenDates(SelectedCompany.Symbol, DateTime.Today.AddDays(-5), DateTime.Today);
            }
            else
            {
               prices = dbService.GetCompleteStockData(SelectedCompany.Symbol);
            }


            ChartData = new PlotModel { Title = $"{SelectedCompany.Name} Stock Prices" };
            ChartData.Series.Clear();
            ChartData.Axes.Clear();

            ChartData.Axes.Add(new DateTimeAxis
            {
                Position = AxisPosition.Bottom,
                StringFormat = "yyyy-MM-dd",
                AxislineStyle = LineStyle.Solid,
                MajorTickSize = 2,
                MinorTickSize = 2,
                IsZoomEnabled = false,
                IsPanEnabled = false

            });

            ChartData.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
                AxislineStyle = LineStyle.LongDashDotDot,
                MajorTickSize = 7,
                MinorTickSize = 4,
                IsZoomEnabled = false,
                IsPanEnabled = false
            });


            var s = new LineSeries();
            foreach (var p in prices)
                s.Points.Add(DateTimeAxis.CreateDataPoint(p.TradeDate, p.ClosePrice));

            ChartData.Series.Add(s);
            ChartData.InvalidatePlot(true);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));




    }
}
