using eCommerceWebApp.Commands;
using eCommerceWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerceWebApp.RestClients
{
    public interface IProductManagementApiClient
    {
        Task<List<Product>> GetProducts();
        Task<Product> GetProductById(int id);
        Task RegisterProduct(RegisterProductCommand command);
    }
}
