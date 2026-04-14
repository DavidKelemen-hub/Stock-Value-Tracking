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

        public static float? CalculateDCF(FinancialStatement model, int years = 5)
        {
            if (model.FreeCashFlow == null || model.SharesOutstanding == null || model.SharesOutstanding == 0)
                return null;

            float fcf = (float)model.FreeCashFlow.Value;

            // Growth rate: blend earnings and revenue growth, cap at 12%
            float rawGrowth = 0f;
            int growthCount = 0;
            if (model.EarningsGrowth.HasValue) { rawGrowth += model.EarningsGrowth.Value; growthCount++; }
            if (model.RevenueGrowth.HasValue) { rawGrowth += model.RevenueGrowth.Value; growthCount++; }
            float g1 = growthCount > 0 ? rawGrowth / growthCount : 0.05f;
            g1 = Math.Clamp(g1, 0f, 0.12f); // Stage 1: capped at 12%
            float g2 = g1 * 0.5f;           // Stage 2: half the initial rate

            // Discount rate via CAPM
            float beta = model.Beta ?? 1.0f;
            beta = Math.Clamp(beta, 0.5f, 3.0f);
            float r = 0.045f + beta * 0.055f;

            // Ensure r > terminal growth to avoid division issues
            float terminalGrowth = 0.03f;
            if (r <= terminalGrowth) r = terminalGrowth + 0.01f;

            // Stage 1: years 1–5 at g1
            float sumPV = 0f;
            float lastFCF = fcf;
            for (int t = 1; t <= years; t++)
            {
                lastFCF *= (1 + g1);
                sumPV += lastFCF / MathF.Pow(1 + r, t);
            }

            // Stage 2: years 6–10 at g2
            for (int t = years + 1; t <= years * 2; t++)
            {
                lastFCF *= (1 + g2);
                sumPV += lastFCF / MathF.Pow(1 + r, t);
            }

            // Terminal value (Gordon Growth) applied after year 10
            float terminalValue = (lastFCF * (1 + terminalGrowth)) / (r - terminalGrowth);
            float tvDiscounted = terminalValue / MathF.Pow(1 + r, years * 2);

            // Net debt adjustment
            float netDebt = (float)((model.TotalDebt ?? 0) - (model.TotalCash ?? 0));

            // Intrinsic value per share
            float equityValue = sumPV + tvDiscounted - netDebt;
            float intrinsicValue = equityValue / model.SharesOutstanding.Value;

            return intrinsicValue > 0 ? (float)Math.Round(intrinsicValue, 2) : null;
        }
    }
}
