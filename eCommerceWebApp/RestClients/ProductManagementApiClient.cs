using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using eCommerceWebApp.Commands;
using eCommerceWebApp.Models;
using Microsoft.Extensions.Configuration;
using System.Web;
using Newtonsoft.Json;

namespace eCommerceWebApp.RestClients
{
    public class ProductManagementApiClient : BaseApiClient, IProductManagementApiClient
    {
        //private readonly IProductManagementApiClient _restClient;
        private readonly HttpClient _httpClient;

        public ProductManagementApiClient(IConfiguration configuration, HttpClient httpClient) : base(configuration)
        {
            _httpClient = httpClient;
            //var productManagementApiAddress = configuration.GetSection("ApiAddress").GetValue<string>("ProductManagementApi");
            _httpClient.BaseAddress = new Uri($"http://{OcelotApiGatewayAddress}/product/");
            //_restClient = RestService.For<IProductManagementApiClient>(httpClient);
        }
        public async Task<Product> GetProductById(int id)
        {
            var productByIdResponse = await _httpClient.GetAsync($"product/{id}");
            if (!productByIdResponse.IsSuccessStatusCode)
                throw new Exception("Could not retrieve the Product");
            var content = await productByIdResponse.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Product>(content);
        }

        public async Task<List<Product>> GetProducts()
        {
            var productsResponse = await _httpClient.GetAsync("product");
            if (!productsResponse.IsSuccessStatusCode)
                throw new Exception("Could not retrieve the Products");
            var content = await productsResponse.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Product>>(content);
        }

        public async Task RegisterProduct(RegisterProductCommand command)
        {
            try
            {
                //await _restClient.RegisterProduct(command);
                var data = JsonConvert.SerializeObject(command);
                var stringContent = new StringContent(data, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("product", stringContent);
                if (!response.IsSuccessStatusCode)
                    throw new Exception("Could not add a product");
                var createdProduct = JsonConvert.DeserializeObject<Product>(await response.Content.ReadAsStringAsync());
            }
            catch(Exception ex)
            {
                
            }
        }
    }
}
