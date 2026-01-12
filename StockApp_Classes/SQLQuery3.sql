WITH bounds AS (
    SELECT
        dp.StockID,
        MIN(dp.TradeDate) AS StartTradeDate,
        MAX(dp.TradeDate) AS EndTradeDate
    FROM dbo.DailyPrices dp
    WHERE dp.TradeDate BETWEEN '2025-01-01' AND '2025-12-31'
    GROUP BY dp.StockID
),
var AS (
    SELECT
        c.StockID,
        c.Symbol,
        c.Name,
        s.ClosePrice AS StartClose,
        e.ClosePrice AS EndClose,
        (e.ClosePrice - s.ClosePrice) AS PriceChange,
        CASE 
            WHEN s.ClosePrice = 0 THEN NULL
            ELSE ((e.ClosePrice - s.ClosePrice) / s.ClosePrice) * 100
        END AS PercentChange
    FROM bounds b
    JOIN dbo.DailyPrices s 
        ON s.StockID = b.StockID 
       AND s.TradeDate = b.StartTradeDate
    JOIN dbo.DailyPrices e 
        ON e.StockID = b.StockID 
       AND e.TradeDate = b.EndTradeDate
    JOIN dbo.Company c     
        ON c.StockID = b.StockID
)
SELECT TOP (10)
    StockID,
    Symbol,
    Name,
    StartClose,
    EndClose,
    PriceChange,
    PercentChange
FROM var
ORDER BY PercentChange ASC;
