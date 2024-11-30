using System.Globalization;
using ChatApp.Domain.Services;
using CsvHelper;
using Microsoft.IdentityModel.Tokens;

namespace ChatApp.Infrastructure.Data.Services;

public class StockQuoteService : IStockQuoteService
{
    public async Task<decimal> GetStockQuoteAsync(string stockCode)
    {
        try
        {
            var url = $"https://stooq.com/q/l/?s={stockCode}&f=sd2t2ohlcv&h&e=csv";
            var response = await new HttpClient().GetStringAsync(url);

            var reader = new StringReader(response);
            var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            var records = csv.GetRecords<StockQuote>().ToList();
            var record = records.FirstOrDefault();
            if(record == null) throw new Exception("Invalid stock code");

            return record.Close;
        }
        catch (Exception ex)
        {
            throw new Exception("Error getting stock quote", ex);
        }
    }

    private class StockQuote
    {
        public string Symbol { get; set; }
        public decimal Close { get; set; }        
    }
}

