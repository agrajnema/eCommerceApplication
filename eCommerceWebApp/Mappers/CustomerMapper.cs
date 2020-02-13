using eCommerceWebApp.Commands;
using eCommerceWebApp.Models.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerceWebApp.Mappers
{
    public static class CustomerMapper
    {
        public static RegisterCustomerCommand MapToRegisterCustomer(this RegisterCustomer customer)
        {
            return new RegisterCustomerCommand(
            Guid.NewGuid(),
            customer.CustomerId,
            customer.FirstName,
            customer.LastName,
            customer.EmailAddress,
            customer.Password);
        }
    }
}
