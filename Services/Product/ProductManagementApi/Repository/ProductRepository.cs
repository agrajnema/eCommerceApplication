using ProductManagementApi.DataAccess;
using ProductManagementApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ProductManagementApi.Repository
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetProducts();
        Task<Product> GetProductById(string productId);
        Task RegisterProduct(Product product);
    }
    public class ProductRepository : IProductRepository
    {
        private readonly ProductDBContext _context;
        public ProductRepository(ProductDBContext context)
        {
            _context = context;
        }

        public async Task<Product> GetProductById(string productId)
        {
            return await _context.Products.FirstOrDefaultAsync(p => p.Id == productId);
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            return  await _context.Products.ToListAsync();
        }
        public async Task RegisterProduct(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }
    }
}
