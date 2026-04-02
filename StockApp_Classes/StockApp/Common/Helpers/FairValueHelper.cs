using Azure.Core;
using StockApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockApp.Common.Helpers
{
    public static class FairValueHelper
    {
        public static decimal? Graham_Value(FinancialStatement st)
        {
            if (st.TrailingEPS == null || st.BookValue == null)
                return null;

            decimal raw = (decimal)(22.5 * st.TrailingEPS.Value * st.BookValue.Value);

            if (raw < 0)
                return null; // or handle differently

            double sqrt = Math.Sqrt((double)raw);

            return decimal.Round((decimal)sqrt, 2);
        }

        public static decimal? PE_Value(FinancialStatement st, decimal? sectorMedianPE)
        {
            if (st.TrailingEPS == null || st.ForwardEPS == null) return null;

            return Decimal.Round((decimal)st.TrailingEPS * (decimal)sectorMedianPE,2);
        }

        public static decimal? EbitdaBased_Value(FinancialStatement st, decimal? sectorMedianEV_EBITDA)
        {
            if (st.EBITDA == null || st.TotalDebt == null || st.TotalCash == null || st.SharesOutstanding == null) return null;
            return Decimal.Round((((decimal)st.EBITDA * (decimal)sectorMedianEV_EBITDA - (decimal)st.TotalDebt + (decimal)st.TotalCash) / (decimal)st.SharesOutstanding),2);
        }

        public static decimal? DividendDiscountModel_Value(FinancialStatement st, double? riskFreeRate)
        {
            if (st.DividendRate == null) return null;
            if (st.Beta == null) return null;
            if (st.EarningsGrowth == null) return null;
            if (st.DividendYield < 1) return null;

            double ERP = 0.055;
            double g = (double)st.EarningsGrowth;
            double discountRate = ((double)riskFreeRate / 100) + (double)st.Beta * ERP;

            if (g >= discountRate)
                g = discountRate - 0.01;  

            double D1 = (double)st.DividendRate * (1 + g);
            double fairValue = D1 / (discountRate - g);

            return Decimal.Round((decimal)fairValue,2);
        }


    }
}
