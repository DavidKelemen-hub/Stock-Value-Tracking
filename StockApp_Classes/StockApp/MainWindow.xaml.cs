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

        public MainWindow(IMainViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}