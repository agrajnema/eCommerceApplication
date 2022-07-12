using eCommerceWebApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eCommerceWebApp.Services
{
    public interface IProductService
    {
        Task<IEnumerable<Models.ProductModel>> GetProduct();
        Task<IEnumerable<Models.ProductModel>> GetProductByCategory(string category);
        Task<Models.ProductModel> GetProduct(string id);
        Task<Models.ProductModel> CreateProduct(Models.ProductModel model);
    }
}
