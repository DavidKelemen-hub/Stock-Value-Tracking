using StockApp.Appl.DTO;
using StockApp.Common.Helpers;
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
        public Task<PerformersDTO> LoadPerformersData(bool isTop10, string selectedRange);
    }
    public class PerformersService : IPerformersService
    {

        private readonly IProcessing _processing;

        public PerformersService(IProcessing _processing)
        {
            this._processing = _processing;
        }

        public async Task<PerformersDTO> LoadPerformersData(bool isTop10, string selectedRange)
        {
            return new PerformersDTO
            {
                Performers = await GetTopPerformers(isTop10, selectedRange),
                PerformersColor = ColorHelper.GetTrendingColor(isTop10),
                PerformerRangeText = DescriptionHelper.GetRangeDescription(1, selectedRange).Substring(2)
            };
        }

        public async Task<List<CompanyPerformance>> GetTopPerformers(bool isTop10, string selectedRange)
        {
            if (isTop10)
            {
                return await _processing.GetTopPerformingCompanies(selectedRange);
            }
            else
            {
                return await _processing.GetLowestPerformingCompanies(selectedRange);
            }
        }
    }
}
