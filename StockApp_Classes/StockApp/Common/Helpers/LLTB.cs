using StockApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SkiaSharp.HarfBuzz.SKShaper;

namespace StockApp.Common.Helpers
{
    public static class LLTB {

        public static List<DailyEntry> LargestTriangleThreeBuckets(List<DailyEntry> data, int threshold)
        {
            NullEntryHelper.SanitizeInput(data);

            int dataLength = data.Count;
            if (threshold >= dataLength || threshold == 0)
                return data;

            var sampled = new List<DailyEntry>(threshold);
            double every = (double)(dataLength - 2) / (threshold - 2);

            int a = 0;
            sampled.Add(data[a]); // Always add the first point

            for (int i = 0; i < threshold - 2; i++)
            {
                // Calculate the range for the next bucket
                int nextBucketStart = (int)Math.Floor((i + 1) * every) + 1;
                int nextBucketEnd = (int)Math.Floor((i + 2) * every) + 1;
                nextBucketEnd = Math.Min(nextBucketEnd, dataLength);

                // Average point in the next bucket (used as point C)
                double avgX = 0, avgY = 0;
                int nextBucketSize = nextBucketEnd - nextBucketStart;
                for (int j = nextBucketStart; j < nextBucketEnd; j++)
                {
                    avgX += data[j].TradeDate.ToOADate();
                    avgY += (double)data[j].ClosePrice;
                }
                avgX /= nextBucketSize;
                avgY /= nextBucketSize;

                // Range for the current bucket
                int currentBucketStart = (int)Math.Floor(i * every) + 1;
                int currentBucketEnd = nextBucketStart;

                // Point A (last selected point)
                double pointAX = data[a].TradeDate.ToOADate();
                double pointAY = (double)data[a].ClosePrice;

                double maxArea = -1;
                int maxAreaIndex = currentBucketStart;

                for (int j = currentBucketStart; j < currentBucketEnd; j++)
                {
                    double pointBX = data[j].TradeDate.ToOADate();
                    double pointBY = (double)data[j].ClosePrice;

                    // Triangle area via cross product
                    double area = Math.Abs(
                        (pointAX - avgX) * (pointBY - pointAY) -
                        (pointAX - pointBX) * (avgY - pointAY)
                    ) * 0.5;

                    if (area > maxArea)
                    {
                        maxArea = area;
                        maxAreaIndex = j;
                    }
                }

                sampled.Add(data[maxAreaIndex]);
                a = maxAreaIndex;
            }

            sampled.Add(data[dataLength - 1]); // Always add the last point
            return sampled;
        }
    }
}
