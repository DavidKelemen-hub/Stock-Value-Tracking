using StockApp.Common.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace StockApp.ViewModels
{
    public class NewsCardViewModel : INotifyPropertyChanged
    {
        public string? Title { get; set; }
        public string? Url { get; set; }
        public string? Thumbnail { get; set; }

        public string DisplayUrl => Uri.TryCreate(Url, UriKind.Absolute, out var uri)
            ? uri.Host.Replace("www.", "")
            : Url ?? "";

        public ICommand OpenUrlCommand => new RelayCommand((_) =>
        {
            if (!string.IsNullOrEmpty(Url))
                Process.Start(new ProcessStartInfo(Url) { UseShellExecute = true });
        });

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
