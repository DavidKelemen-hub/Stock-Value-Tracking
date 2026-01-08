using OpenTK.Graphics.ES10;
using StockApp_Classes.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockApp.Helpers
{
    public class SearchHelper
    {
        public List<Company> GetMatchingCompanies(string searchString, ObservableCollection<Company> companies)
        {
            List<Company> matchingCompanies = new List<Company>();
            foreach (var item in companies)
            {
                if( (item.Name.ToLower().StartsWith(searchString) || item.Name.ToLower().Contains(searchString) ) ||
                    (item.Symbol.ToLower().StartsWith(searchString) || item.Symbol.ToLower().Contains(searchString) ) )
                {
                    matchingCompanies.Add(item);
                }
            }
            return matchingCompanies;
        }
        
    }
}
