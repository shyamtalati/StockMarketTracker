using Microsoft.AspNetCore.Mvc.RazorPages;
using StockMarketTracker.services;
using StockMarketTracker.Models;
using System.Linq;
using Newtonsoft.Json;

namespace StockMarketTracker.Pages
{
    public class IndexModel : PageModel
    {
        private readonly StockService _stockService;

        public IndexModel(StockService stockService)
        {
            _stockService = stockService;
        }

        public List<HistoricalStockData> HistoricalStockData { get; set; } = new List<HistoricalStockData>();

        public async Task OnGetAsync(string symbol)
        {
            if (!string.IsNullOrEmpty(symbol))
            {
                HistoricalStockData = await _stockService.GetHistoricalDataAsync(symbol);
            }
        }
    }
}
