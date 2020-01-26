using CustomerManagementApi.DataAccess;
using CustomerManagementApi.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerManagementApi.Repository
{
    public interface ICustomerRepository 
    {
        Task<IEnumerable<Customer>> GetAll();
        Task<Customer> GetById(int customerId);
        Task<Customer> GetByEmailId(string emailID);
        Task<Customer> Create(Customer customer);
    }

    public class CustomerRepository : ICustomerRepository
    {
        private readonly CustomerDBContext _context;
        public CustomerRepository(CustomerDBContext context)
        {
            _context = context;
        }

        public async Task<Customer> Create(Customer customer)
        {
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            return customer;
        }

        public async Task<IEnumerable<Customer>> GetAll()
        {
            return await _context.Customers.ToListAsync();
        }

        public async Task<Customer> GetByEmailId(string emailID)
        {
            return await _context.Customers.FirstOrDefaultAsync(cust => cust.EmailAddress == emailID);
        }

        public async Task<Customer> GetById(int customerId)
        {
            return await _context.Customers.FirstOrDefaultAsync(cust => cust.CustomerId == customerId);
        }
    }
}
