using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace OrderService.Infra.HttpClients
{
    public class ProductHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly IDistributedCache _cache;
        private readonly ILogger<ProductHttpClient> _logger;

        public ProductHttpClient(
            HttpClient httpClient,
            IDistributedCache cache,
            ILogger<ProductHttpClient> logger)
        {
            _httpClient = httpClient;
            _cache = cache;
            _logger = logger;
        }

        public async Task<ProductDto> GetProductAsync(int productId)
        {
            var cacheKey = $"product:{productId}";
            var cached = await _cache.GetStringAsync(cacheKey);

            if (cached != null)
            {
                _logger.LogInformation("✅ Redis HIT → {key}", cacheKey);
                return JsonSerializer.Deserialize<ProductDto>(cached)!;
            }

            _logger.LogWarning("❌ Redis MISS → {key}", cacheKey);

            var response = await _httpClient.GetAsync($"api/products/{productId}");
            if (!response.IsSuccessStatusCode)
                throw new ApplicationException("Product service unavailable");

            var json = await response.Content.ReadAsStringAsync();
            var product = JsonSerializer.Deserialize<ProductDto>(json)!;

            await _cache.SetStringAsync(
                cacheKey,
                JsonSerializer.Serialize(product),
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                });

            _logger.LogInformation("📦 Redis SET → {key}", cacheKey);

            return product;
        }

        public class ProductDto
        {
            public int Id { get; set; }
            public decimal Price { get; set; }
        }
    }
}