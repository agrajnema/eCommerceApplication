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
        Task Create(Customer customer);
    }

    public class CustomerRepository : ICustomerRepository
    {
        private readonly CustomerDBContext _context;
        public CustomerRepository(CustomerDBContext context)
        {
            _context = context;
        }

        public async Task Create(Customer customer)
        {
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            //return customer;
        }

        public async Task<IEnumerable<Customer>> GetAll()
        {
            return await _context.Customers.ToListAsync();
        }

        public async Task<Customer> GetByEmailId(string emailID)
        {
            if (!_context.Customers.Any())                
                return null;

            return await _context.Customers.Where(cust => cust.EmailAddress == emailID).FirstOrDefaultAsync();
        }

        public async Task<Customer> GetById(int customerId)
        {
            return await _context.Customers.Where(cust => cust.CustomerId == customerId).FirstOrDefaultAsync();
        }
    }
}
