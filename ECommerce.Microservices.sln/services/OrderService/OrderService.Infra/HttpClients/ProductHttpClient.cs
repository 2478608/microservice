using System.Text.Json;

namespace OrderService.Infra.HttpClients
{
    public  class ProductHttpClient
    {
        private readonly HttpClient _httpClient;

        public ProductHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ProductDto> GetProductAsync(int  productId)
        {
            var response = await _httpClient.GetAsync($"api/products/{productId}");

            if (!response.IsSuccessStatusCode)
                throw new ApplicationException("Product service unavailable");

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ProductDto>(json)!;

        }

        public class ProductDto
        {
            public int Id { get; set; }
            public decimal Price { get; set; }
        }
    }
}
