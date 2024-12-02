using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;
using StockMarketTracker.Models;

namespace StockMarketTracker.services
{
    public class StockService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public StockService(HttpClient httpClient, string apiKey)
        {
            _httpClient = httpClient;
            _apiKey = apiKey;
        }

        public async Task<List<HistoricalStockData>> GetHistoricalDataAsync(string symbol)
        {
            var currentDate = DateTime.Now.ToString("yyyy-MM-dd");
            var url = $"https://api.polygon.io/v2/aggs/ticker/{symbol}/range/1/day/2023-01-01/{currentDate}?apiKey={_apiKey}";
            var response = await _httpClient.GetStringAsync(url);
            var historicalDataResponse = JsonConvert.DeserializeObject<HistoricalStockDataResponse>(response);

            var historicalData = new List<HistoricalStockData>();
            foreach (var result in historicalDataResponse.Results)
            {
                historicalData.Add(new HistoricalStockData
                {
                    Date = DateTimeOffset.FromUnixTimeMilliseconds(result.Timestamp).DateTime,
                    Open = result.Open,
                    Close = result.Close,
                    High = result.High,
                    Low = result.Low,
                    Volume = result.Volume
                });
            }

            return historicalData;
        }
    }

    public class HistoricalStockDataResponse
    {
        [JsonProperty("results")]
        public List<HistoricalStockDataResult> Results { get; set; } = new List<HistoricalStockDataResult>();
    }

    public class HistoricalStockDataResult
    {
        [JsonProperty("t")]
        public long Timestamp { get; set; }

        [JsonProperty("o")]
        public decimal Open { get; set; }

        [JsonProperty("c")]
        public decimal Close { get; set; }

        [JsonProperty("h")]
        public decimal High { get; set; }

        [JsonProperty("l")]
        public decimal Low { get; set; }

        [JsonProperty("v")]
        public long Volume { get; set; }
    }
}
