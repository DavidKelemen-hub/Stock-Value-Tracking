using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace StockApp.Common.Helpers
{
    public static class LogoHelper
    {
        public static ImageSource GetCompanyLogo(string symbol)
        {
            ImageSource companyLogo;
            Uri uri;
            try
            {
                uri = new Uri($"pack://application:,,,/StockApp;component/Presentation/Resources/{symbol.ToLower()}.png", UriKind.Absolute);
                companyLogo = new BitmapImage(uri);
            }
            catch (IOException ex)
            {
                uri = new Uri($"pack://application:,,,/StockApp;component/Presentation/Resources/notfound.png", UriKind.Absolute);
                companyLogo = new BitmapImage(uri);
            }
            return companyLogo;
        }
    }
}
