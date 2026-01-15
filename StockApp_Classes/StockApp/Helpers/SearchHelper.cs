using StockApp.Models;
using System.Collections.ObjectModel;

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
