using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json.Linq;
using RestSharp;

public class CryptoDataService : BackgroundService
{
    private readonly IHubContext<CryptoHub> _hubContext;

    public CryptoDataService(IHubContext<CryptoHub> hubContext)
    {
        _hubContext = hubContext;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var client = new RestClient("https://api.binance.com");
            var request = new RestRequest("/api/v3/ticker/price?symbol=BTCUSDT", Method.Get);

            var response = await client.ExecuteAsync(request, stoppingToken);
            if (response.IsSuccessful)
            {
                var jsonResponse = JObject.Parse(response.Content);
                var btcUsdtPrice = jsonResponse["price"].Value<string>();

                await _hubContext.Clients.All.SendAsync("ReceivePriceUpdate", btcUsdtPrice, stoppingToken);
            }

            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
        }
    }
}
