using System;

namespace ChatApp.Domain.Services;

public interface IStockQuoteService
{
    Task<decimal> GetStockQuoteAsync(string stockCode);
}
