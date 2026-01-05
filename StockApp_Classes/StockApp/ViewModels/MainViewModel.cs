using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StockApp_Classes.Services;

namespace StockApp.ViewModels
{
    public class MainViewModel
    {
        public CompaniesViewModel CompaniesViewModel { get; set; }
        public DataBaseService _service { get; set; }
        public MainViewModel()
        {
            _service = new DataBaseService(System.Configuration.ConfigurationManager.ConnectionStrings["dbconnection"].ConnectionString);
            CompaniesViewModel = new CompaniesViewModel(_service);
        }
    }
}
