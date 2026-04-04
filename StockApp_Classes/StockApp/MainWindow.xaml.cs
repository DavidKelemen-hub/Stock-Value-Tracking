using StockApp.Appl.Services;
using StockApp.Domain.Processing;
using StockApp.Infrastructure.DataAccess;
using StockApp.ViewModels;
using System.Diagnostics;
using System.Windows;

namespace StockApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Process? _fastapiProcess;
        public MainWindow(IMainViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
            StartFastApi();
            Closing += (s, e) => StopFastApi();
        }

        private void StartFastApi()
        {
            _fastapiProcess = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "python",
                    Arguments = "-m uvicorn main:app --host 127.0.0.1 --port 8000",
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    WorkingDirectory = @"C:\Repos\YFinance-API\Fetch Stock Information\Stock News"
                }
            };
            _fastapiProcess.Start();
            Thread.Sleep(2000);
        }

        private void StopFastApi()
        {
            if (_fastapiProcess != null && !_fastapiProcess.HasExited)
            {
                _fastapiProcess.Kill();
                _fastapiProcess.Dispose();
            }
        }
    }
}