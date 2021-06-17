using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using ProductManagementApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductManagementApi.DataAccess
{
    public interface IProductDBMongoContext
    {
        IMongoCollection<Product> Products { get; }
    }
    public class ProductDBMongoContext : IProductDBMongoContext
    {
        public ProductDBMongoContext(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            var database = client.GetDatabase(configuration.GetValue<string>("DatabaseSettings:DatabaseName"));

            Products = database.GetCollection<Product>(configuration.GetValue<string>("DatabaseSettings:CollectionName"));
            ProductDBMongoContextSeed.SeedData(Products);
        }
        public IMongoCollection<Product> Products {get;}
    }
}
