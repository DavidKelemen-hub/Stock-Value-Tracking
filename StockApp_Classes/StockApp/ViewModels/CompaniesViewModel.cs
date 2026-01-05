using StockApp.ViewModels;
using StockApp_Classes.Models;
using StockApp_Classes.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace StockApp
{
    public class CompaniesViewModel
    {
        public ObservableCollection<Company> Companies { get; set; }
        
        public CompaniesViewModel(DataBaseService service)
        {
            Companies = new ObservableCollection<Company>(service.GetAllCompanies());
        }

        private Company selectedCompany;

        public Company SelectedCompany
        {
            get { return selectedCompany; }
            set {  selectedCompany = value; }
        }
    }
}
