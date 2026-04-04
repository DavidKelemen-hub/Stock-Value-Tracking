using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace StockApp
{
    /// <summary>
    /// Interaction logic for SplashScreen.xaml
    /// </summary>
    public partial class SplashScreenWindow : Window
    {
        public SplashScreenWindow()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            var fadeIn = (Storyboard)Resources["FadeIn"];
            fadeIn.Begin();
        }

        public async Task CloseWithFade()
        {
            var fadeOut = (Storyboard)Resources["FadeOut"];
            var tcs = new TaskCompletionSource<bool>();
            fadeOut.Completed += (s, e) => tcs.SetResult(true);
            fadeOut.Begin();
            await tcs.Task;
            Close();
        }

        public void SetStatus(string text)
        {
            StatusText.Text = text;
        }
    }
}
