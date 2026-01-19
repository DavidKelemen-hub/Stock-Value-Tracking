using StockApp.Appl.DTO;
using StockApp.Domain.Models;
using StockApp.Domain.Processing;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace StockApp.Appl.Services
{
    public interface IPerformersService
    {
        public PerformersDTO LoadPerformersData(bool isTop10, string selectedRange);
    }
    public class PerformersService : IPerformersService
    {

        private readonly IProcessing _processing;

        public PerformersService(IProcessing _processing)
        {
            this._processing = _processing;
        }

        public PerformersDTO LoadPerformersData(bool isTop10, string selectedRange)
        {
            var performers = GetTopPerformers(isTop10, selectedRange);
            var performersColor = GetPerformersColor(isTop10);
            var rangeText = _processing.GetRangeDescription(1, selectedRange);
            var performerRangeText = rangeText.Substring(2);

            return new PerformersDTO
            {
                Performers = performers,
                PerformersColor = performersColor,
                PerformerRangeText = performerRangeText
            };
        }

        public List<CompanyPerformance> GetTopPerformers(bool isTop10, string selectedRange)
        {
            if (isTop10)
            {
                return _processing.GetTopPerformingCompanies(selectedRange);
            }
            else
            {
                return _processing.GetLowestPerformingCompanies(selectedRange);
            }
        }

        public Brush GetPerformersColor(bool isTop10)
        {
            return isTop10 ? new SolidColorBrush(Colors.LimeGreen) : new SolidColorBrush(Colors.IndianRed);
        }
    }
}
