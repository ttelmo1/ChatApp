using ChatApp.Infraestructure.Models;
using System;

namespace ChatApp.Infraestructure.Services.Interfaces;

public interface IStockQuoteService
{
    Task<ResultViewModel<decimal>> GetStockQuoteAsync(string stockCode);
}
