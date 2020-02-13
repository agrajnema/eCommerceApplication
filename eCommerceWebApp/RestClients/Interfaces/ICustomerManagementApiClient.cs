using eCommerceWebApp.Commands;
using eCommerceWebApp.Models.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerceWebApp.RestClients
{
    public interface ICustomerManagementApiClient
    {
        Task RegisterCustomer(RegisterCustomerCommand customer);
        Task<Customer> AuthenticateCustomer(AuthenticateCustomer customer);
    }
}
