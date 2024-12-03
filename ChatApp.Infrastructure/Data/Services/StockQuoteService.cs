using System.Globalization;
using ChatApp.Infraestructure.Models;
using ChatApp.Infraestructure.Services.Interfaces;
using CsvHelper;

namespace ChatApp.Infrastructure.Data.Services;

public class StockQuoteService : IStockQuoteService
{
    public async Task<ResultViewModel<decimal>> GetStockQuoteAsync(string stockCode)
    {
        try
        {
            var url = $"https://stooq.com/q/l/?s={stockCode}&f=sd2t2ohlcv&h&e=csv";
            var response = await new HttpClient().GetStringAsync(url);

            var reader = new StringReader(response);
            var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            var records = csv.GetRecords<StockQuote>().ToList();
            var record = records.FirstOrDefault();
            if(record == null)
            {
                return ResultViewModel<decimal>.Error($"Stock code {stockCode.ToUpper()} not found");
            }

            return ResultViewModel<decimal>.Success(record.Close);
        }
        catch (Exception ex)
        {
            if(ex.Message.StartsWith("The conversion cannot be performed."))
            {
                return ResultViewModel<decimal>.Error($"Stock code {stockCode.ToUpper()} not found");
            }
            return ResultViewModel<decimal>.Error(ex.Message);
        }
    }

    private class StockQuote
    {
        public string Symbol { get; set; }
        public decimal Close { get; set; }        
    }
}

