using StockApp.DataBaseServices;
using StockApp.ProcessingService;
using StockApp.StockService;
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
            IService service = new Service(processing); 

            DataContext = new MainViewModel(service);
        }



    }
}