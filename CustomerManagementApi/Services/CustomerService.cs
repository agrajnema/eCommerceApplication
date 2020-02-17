using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomerManagementApi.Model;
using CustomerManagementApi.Repository;

namespace CustomerManagementApi.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }
        public async Task<Customer> Authenticate(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
                throw new Exception("Email ID and Password are required");

            var customer = await GetByEmailId(userName);
            if (customer == null)
                throw new Exception("User does not exist");

            if (!VerifyPasswordHash(password, customer.PasswordHash, customer.PasswordSalt))
                throw new Exception("User name or password is incorrect");

            return customer;
        }

        public async Task<Customer> Create(Customer customer, string password)
        {
            if (string.IsNullOrEmpty(password))
                throw new Exception("Password is required");

            var doesCustomerExist = await GetByEmailId(customer.EmailAddress);
            if (doesCustomerExist != null)
                throw new Exception("User with the email address already exist");

            Console.WriteLine("Response received from customer email id check");

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            customer.PasswordHash = passwordHash;
            customer.PasswordSalt = passwordSalt;

            Console.WriteLine(customer.FirstName);
            Console.WriteLine(customer.LastName);
            Console.WriteLine(customer.EmailAddress);
            Console.WriteLine(customer.PasswordSalt);
            Console.WriteLine(customer.PasswordHash);
            await _customerRepository.Create(customer);
            return customer;

        }

        public async Task<IEnumerable<Customer>> GetAll()
        {
            return await _customerRepository.GetAll();
        }

        public async Task<Customer> GetById(int customerId)
        {
            return await _customerRepository.GetById(customerId);
        }

        public async Task<Customer> GetByEmailId(string emailId)
        {
            return await _customerRepository.GetByEmailId(emailId);
        }
        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (string.IsNullOrEmpty(password))
                throw new ArgumentException("Password cannot be null or empty");

            using (var hmacPassword = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmacPassword.Key;
                passwordHash = hmacPassword.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            if (string.IsNullOrEmpty(password))
                throw new ArgumentException("Password cannot be null or empty");

            if (passwordHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (passwordSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmacPassword = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedPasswordHash = hmacPassword.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedPasswordHash.Length; i++)
                {
                    if (passwordHash[i] != computedPasswordHash[i]) return false;
                }
            }
            return true;
        }
    }
}
