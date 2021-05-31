using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using eCommerceWebApp.Commands;
using eCommerceWebApp.Infrastructure;
using eCommerceWebApp.Models.Customer;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace eCommerceWebApp.RestClients
{
    public class CustomerManagementApiClient : BaseHttpClientWithFactory, ICustomerManagementApiClient
    {
        private readonly IConfiguration _configuration;

        public CustomerManagementApiClient(IHttpClientFactory factory, IConfiguration configuration) : base(factory)
        {
            _configuration = configuration;
            OcelotApiGatewayAddress = configuration.GetSection("ApiAddress").GetValue<string>("OcelotApiGateway");
        }
        public async Task<Customer> AuthenticateCustomer(AuthenticateCustomer customer)
        {
            var message = new HttpRequestBuilder(OcelotApiGatewayAddress)
                              .SetPath("/customer")
                              .AddToPath("authenticate")
                              .HttpMethod(HttpMethod.Post)
                              .GetHttpMessage();

            var json = JsonConvert.SerializeObject(customer);
            message.Content = new StringContent(json, Encoding.UTF8, "application/json");

            return await SendRequest<Customer>(message);
        }

        public async Task RegisterCustomer(RegisterCustomerCommand customer)
        {
            try
            {
                var message = new HttpRequestBuilder(OcelotApiGatewayAddress)
                                  .SetPath("/customer")
                                  .AddToPath("register")
                                  .HttpMethod(HttpMethod.Post)
                                  .GetHttpMessage();

                var json = JsonConvert.SerializeObject(customer);
                message.Content = new StringContent(json, Encoding.UTF8, "application/json");

                await SendRequest<Customer>(message);
            }
            catch(Exception ex)
            {

            }
        }
    }
}
