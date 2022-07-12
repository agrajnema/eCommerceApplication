using eCommerceWebApp.Extensions;
using eCommerceWebApp.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace eCommerceWebApp.Services
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _client;

        public ProductService(HttpClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<IEnumerable<Models.ProductModel>> GetProduct()
        {
            var response = await _client.GetAsync("/Product");
            return await response.ReadContentAs<List<Models.ProductModel>>();
        }

        public async Task<Models.ProductModel> GetProduct(string id)
        {
            var response = await _client.GetAsync($"/Product/{id}");
            return await response.ReadContentAs<Models.ProductModel>();
        }

        public async Task<IEnumerable<Models.ProductModel>> GetProductByCategory(string category)
        {
            var response = await _client.GetAsync($"/Product/GetProductByCategory/{category}");
            return await response.ReadContentAs<List<Models.ProductModel>>();
        }

        public async Task<Models.ProductModel> CreateProduct(Models.ProductModel model)
        {
            var response = await _client.PostAsJson($"/Product", model);
            if (response.IsSuccessStatusCode)
                return await response.ReadContentAs<Models.ProductModel>();
            else
            {
                throw new Exception("Something went wrong when calling api.");
            }
        }
    }
}
