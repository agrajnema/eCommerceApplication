using CustomerManagementApi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerManagementApi.Services
{
    public interface ICustomerService
    {
        Task<Customer> Authenticate(string userName, string password);
        Task<IEnumerable<Customer>> GetAll();
        Task<Customer> GetById(int customerId);
        Task<Customer> GetByEmailId(string emailId);
        Task<Customer> Create(Customer customer, string password);
    }
}
