using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using SpotPriceBridge.Data;
using SpotPriceBridge.Models;

namespace SpotPriceBridge.Services
{
    public class SpotPriceUpdater : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<SpotPriceUpdater> _logger;
        private readonly HttpClient _httpClient;

        private readonly string _apiUrl = "https://stage-connect.fiztrade.com/FizServices/GetSpotPriceData/102-ff9457612ee1f463132c0d8a465cd95d";

        public SpotPriceUpdater(IServiceProvider serviceProvider, ILogger<SpotPriceUpdater> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _httpClient = new HttpClient();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var response = await _httpClient.GetAsync(_apiUrl, stoppingToken);
                    response.EnsureSuccessStatusCode();

                    var content = await response.Content.ReadAsStringAsync();
                    var json = JsonDocument.Parse(content).RootElement;

                    var now = DateTime.UtcNow;

                    var entries = new[]
                    {
                        new SpotPriceModel { Code = "G", AskPrice = json.GetProperty("goldAsk").GetDecimal(), UpdatedOnUtc = now },
                        new SpotPriceModel { Code = "S", AskPrice = json.GetProperty("silverAsk").GetDecimal(), UpdatedOnUtc = now },
                        new SpotPriceModel { Code = "P", AskPrice = json.GetProperty("platinumAsk").GetDecimal(), UpdatedOnUtc = now },
                        new SpotPriceModel { Code = "L", AskPrice = json.GetProperty("palladiumAsk").GetDecimal(), UpdatedOnUtc = now }
                    };

                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                        db.NewSpotPrice.RemoveRange(db.NewSpotPrice);
                        await db.NewSpotPrice.AddRangeAsync(entries, stoppingToken);
                        await db.SaveChangesAsync(stoppingToken);
                    }

                    _logger.LogInformation("Spot prices updated at {Time}", now);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error updating spot prices");
                }

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
    }
}
