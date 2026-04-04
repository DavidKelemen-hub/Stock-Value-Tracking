using Microsoft.Extensions.DependencyInjection;
using StockApp.Appl.Services;
using StockApp.Domain.Processing;
using StockApp.Infrastructure.DataAccess;
using StockApp.ViewModels;
using System.Configuration;
using System.Data;
using System.Windows;

namespace StockApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IServiceProvider? _serviceProvider;

        protected override async void OnStartup(StartupEventArgs e)
        {
            var splash = new SplashScreenWindow();
            splash.Show();

            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            _serviceProvider = serviceCollection.BuildServiceProvider();

            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();

            // simulate loading while services are being built
            await Task.Delay(5000);

            mainWindow.Show();

            await splash.CloseWithFade();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            // Configure Logging
            services.AddLogging();

            // Register Services
            services.AddSingleton<IDataBaseService, DataBaseService>();
            services.AddSingleton<IProcessing, Processing>();
            services.AddSingleton<IStockService, StockService>();
            services.AddSingleton<IMessageService, MessageService>();

            // Register ViewModels
            services.AddSingleton<IMainViewModel, MainViewModel>();

            // Register Views
            services.AddSingleton<MainWindow>();
        }

        private void OnExit(object sender, ExitEventArgs e)
        {
            // Dispose of services if needed
            if (_serviceProvider is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }
    }

}
