using Shopping.Aggregator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shopping.Aggregator.Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductModel>> GetProducts();
        Task<IEnumerable<ProductModel>> GetProductByCategory(string category);
        Task<ProductModel> GetProduct(string id);
    }
}
