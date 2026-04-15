using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using ProductService.Core.DTOs;
using ProductService.Core.Interfaces.Repositories;
using ProductService.Core.Interfaces.Services;

namespace ProductService.Infra.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repo;
        private readonly IDistributedCache _cache;
        private readonly ILogger<ProductService> _logger;

        public ProductService(
            IProductRepository repo,
            IDistributedCache cache,
            ILogger<ProductService> logger)
        {
            _repo = repo;
            _cache = cache;
            _logger = logger;
        }

        public ProductDto GetProductById(int id)
        {
            var cacheKey = $"product:{id}";
            var cached = _cache.GetString(cacheKey);

            if (cached != null)
            {
                _logger.LogInformation("✅ Redis HIT → {key}", cacheKey);
                return JsonSerializer.Deserialize<ProductDto>(cached)!;
            }

            _logger.LogWarning("❌ Redis MISS → {key}", cacheKey);

            var prod = _repo.GetById(id)
                ?? throw new Exception("Product not found");

            var dto = new ProductDto
            {
                Id = prod.Id,
                Name = prod.Name,
                Price = prod.Price
            };

            _cache.SetString(
                cacheKey,
                JsonSerializer.Serialize(dto),
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                });

            _logger.LogInformation("📦 Redis SET → {key}", cacheKey);

            return dto;
        }

        public void InvalidateProductCache(int id)
        {
            var cacheKey = $"product:{id}";
            _cache.Remove(cacheKey);
            _logger.LogInformation("🧹 Redis REMOVE → {key}", cacheKey);
        }
    }
}
