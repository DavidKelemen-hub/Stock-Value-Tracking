using StockApp.Appl.Services;
using StockApp.Domain.Processing;
using StockApp.Infrastructure.DataAccess;
using StockApp.ViewModels;
using System.Windows;

namespace StockApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();

            IDataBaseService db = new DataBaseService();
            IProcessing processing = new Processing(db);
            IStockService stockService = new StockService(processing);
            IPerformersService performersService = new PerformersService(processing);

            DataContext = new MainViewModel(stockService, performersService);
        }
    }
}