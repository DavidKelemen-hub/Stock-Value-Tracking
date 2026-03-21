using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockApp.Domain.Models
{
    public class Company
    {
        public int StockID { get; set; }
        public string? Symbol { get; set; }
        public string? Name { get; set; }
    }
}
