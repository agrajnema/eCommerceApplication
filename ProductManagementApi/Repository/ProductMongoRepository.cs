using MongoDB.Driver;
using ProductManagementApi.DataAccess;
using ProductManagementApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductManagementApi.Repository
{
    public interface IProductMongoRepository
    {
        Task<IEnumerable<Product>> GetProducts();
        Task<Product> GetProduct(string id);
        Task<IEnumerable<Product>> GetProductByName(string name);
        Task<IEnumerable<Product>> GetProductByCategory(string categoryName);

        Task CreateProduct(Product product);
        Task<bool> UpdateProduct(Product product);
        Task<bool> DeleteProduct(string id);
    }
    public class ProductMongoRepository : IProductMongoRepository
    {
        private readonly IProductDBMongoContext _context; 
        public ProductMongoRepository(IProductDBMongoContext context)
        {
            _context = context;
        }
        public async Task CreateProduct(Product product)
        {
            await _context.Products.InsertOneAsync(product);
        }

        public async Task<bool> DeleteProduct(string id)
        {
            var deleteResult = await _context.Products.DeleteOneAsync<Product>(filter: p => p.Id == id);
            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
        }

        public async Task<Product> GetProduct(string id)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Id, id);
            return await _context.Products.Find(filter).FirstOrDefaultAsync();


        }

        public async Task<IEnumerable<Product>> GetProductByCategory(string categoryName)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Category, categoryName);
            return await _context.Products.Find(filter).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductByName(string name)
        {
            return await _context.Products.Find(p => p.Name == name).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _context
                             .Products
                             .Find(p => true)
                             .ToListAsync();
        }

        public async Task<bool> UpdateProduct(Product product)
        {
            var updateResult = await _context.Products.ReplaceOneAsync<Product>(filter: p => p.Id == product.Id, replacement: product);
            return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
        }
    }
}
