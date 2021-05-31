using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using eCommerceWebApp.Commands;
using Microsoft.Extensions.Configuration;
using System.Web;
using Newtonsoft.Json;
using eCommerceWebApp.Models.Product;
using Microsoft.AspNetCore.Http;
using eCommerceWebApp.Infrastructure;

namespace eCommerceWebApp.RestClients
{
    
    public class ProductManagementApiClient : BaseHttpClientWithFactory, IProductManagementApiClient
    {
        //private readonly IProductManagementApiClient _restClient;
        //private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public ProductManagementApiClient(IHttpClientFactory factory, IConfiguration configuration) : base(factory)
        {
            _configuration = configuration;
            OcelotApiGatewayAddress = configuration.GetSection("ApiAddress").GetValue<string>("OcelotApiGateway");
        }
        public async Task<Product> GetProductById(int id)
        {

            var message = new HttpRequestBuilder(OcelotApiGatewayAddress)
                              .SetPath("/product")
                              .AddToPath(Convert.ToString(id))
                              .HttpMethod(HttpMethod.Get)
                              .GetHttpMessage();

            return await SendRequest<Product>(message);
        }

        public async Task<List<Product>> GetProducts()
        {
            try
            {
                var message = new HttpRequestBuilder(OcelotApiGatewayAddress)
                              .SetPath("/product")
                              .HttpMethod(HttpMethod.Get)
                              .GetHttpMessage();

                return await SendRequest<List<Product>>(message);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task RegisterProduct(RegisterProductCommand command)
        {
            try
            {
                var message = new HttpRequestBuilder(OcelotApiGatewayAddress)
                                   .SetPath("/product")
                                   .HttpMethod(HttpMethod.Post)
                                   .GetHttpMessage();

                var json = JsonConvert.SerializeObject(command);
                message.Content = new StringContent(json, Encoding.UTF8, "application/json");

                await SendRequest<Product>(message);
            }
            catch(Exception ex)
            {

            }
        }
    }
}
