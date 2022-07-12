using Shopping.Aggregator.Extensions;
using Shopping.Aggregator.Models;
using Shopping.Aggregator.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Shopping.Aggregator.Services
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _client;

        public ProductService(HttpClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<IEnumerable<ProductModel>> GetProducts()
        {
            var response = await _client.GetAsync("/api/v1/Product");
            return await response.ReadContentAsync<List<ProductModel>>();
        }

        public async Task<ProductModel> GetProduct(string id)
        {
            var response = await _client.GetAsync($"/api/v1/Product/{id}");
            return await response.ReadContentAsync<ProductModel>();
        }

        public async Task<IEnumerable<ProductModel>> GetProductByCategory(string category)
        {
            var response = await _client.GetAsync($"/api/v1/Product/GetProductByCategory/{category}");
            return await response.ReadContentAsync<List<ProductModel>>();
        }
    }
}
