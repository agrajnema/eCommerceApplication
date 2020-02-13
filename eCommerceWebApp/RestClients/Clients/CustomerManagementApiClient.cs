using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using eCommerceWebApp.Commands;
using eCommerceWebApp.Models.Customer;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace eCommerceWebApp.RestClients
{
    public class CustomerManagementApiClient : BaseApiClient, ICustomerManagementApiClient
    {
        private readonly HttpClient _httpClient;

        public CustomerManagementApiClient(IConfiguration configuration, HttpClient httpClient): base(configuration)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri($"http://{OcelotApiGatewayAddress}/customer/");
        }
        public async Task<Customer> AuthenticateCustomer(AuthenticateCustomer customer)
        {
            var data = JsonConvert.SerializeObject(customer);
            var stringData = new StringContent(data, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("authenticate", stringData);
            if (!response.IsSuccessStatusCode)
                throw new Exception("Could not authenticate");
            var authenticatedCustomer = JsonConvert.DeserializeObject<Customer>(await response.Content.ReadAsStringAsync());
            return authenticatedCustomer;
        }

        public async Task RegisterCustomer(RegisterCustomerCommand customer)
        {
            var data = JsonConvert.SerializeObject(customer);
            var stringData = new StringContent(data, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("register", stringData);
            if (!response.IsSuccessStatusCode)
                throw new Exception("Could not register");
        }
    }
}
