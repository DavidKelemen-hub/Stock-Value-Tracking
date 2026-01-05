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


namespace StockApp.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {     
        public ObservableCollection<Company> Companies { get; set; }
        private Company selectedCompany;
        private PlotModel chartData;
        private DataBaseService dbService { get; set; }

        public MainViewModel()
        {
            dbService = new DataBaseService(System.Configuration.ConfigurationManager.ConnectionStrings["dbconnection"].ConnectionString);
            Companies = new ObservableCollection<Company>(dbService.GetAllCompanies());
        }

        public Company SelectedCompany
        {
            get { return selectedCompany; }
            set
            {
                if (selectedCompany == value) return;
                selectedCompany = value;
                LoadChartData();
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

        private void LoadChartData()
        {
            if (SelectedCompany == null)
                return;

            var prices = dbService.GetStockEntriesBetweenDates(SelectedCompany.Symbol,"2025-01-01","2025-12-31");

            ChartData = new PlotModel { Title = $"{SelectedCompany.Name} Stock Prices" };
            ChartData.Series.Clear();
            ChartData.Axes.Clear();

            ChartData.Axes.Add(new DateTimeAxis
            {
                Position = AxisPosition.Bottom,
                StringFormat = "yyyy-MM-dd",
                AxislineStyle = LineStyle.Solid,
                MajorTickSize = 2,
                MinorTickSize = 2
            });

            ChartData.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
                AxislineStyle = LineStyle.LongDashDotDot,
                MajorTickSize = 7,
                MinorTickSize = 4
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
