using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerceWebApp.RestClients
{
    public class BaseApiClient
    {
        public BaseApiClient(IConfiguration configuration)
        {
            OcelotApiGatewayAddress = configuration.GetSection("ApiAddress").GetValue<string>("OcelotApiGateway");
        }
        public string OcelotApiGatewayAddress { get; set; }
        public static string JWTToken { get; set; }
    }
}
