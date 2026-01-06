using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockApp.Helpers
{
    public class DateTimeHelper
    {
        private string range { get; set; }
        public DateTime startDate { get; set; }

        public DateTimeHelper()
        {
            this.startDate = DateTime.Today;
        }

        public DateTime GetStartDate(string range)
        {
            this.range = range;
            switch (this.range)
            {
                case "5D":
                    this.startDate = DateTime.Today.AddDays(-5);
                    break;
                case "1M":
                    this.startDate = DateTime.Today.AddMonths(-1);
                    break;
                case "3M":
                    this.startDate = DateTime.Today.AddMonths(-3);
                    break;
                case "6M":
                    this.startDate = DateTime.Today.AddMonths(-6);
                    break;
                case "1Y":
                    this.startDate = DateTime.Today.AddYears(-1);
                    break;
                case "5Y":
                    this.startDate = DateTime.Today.AddYears(-5);
                    break;
                default:
                    this.startDate = DateTime.Today.AddMonths(-1);
                    break;
            }
            return this.startDate;
        }

    }
}
