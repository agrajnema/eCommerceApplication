using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProductManagementApi.DataAccess;

namespace ProductManagementApi.Models
{
    public static class DBSeeder
    {
        public static void PopulateDB(IApplicationBuilder builder)
        {
            using(var serviceScope = builder.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<ProductDBContext>());
            }
        }

        public static void SeedData(ProductDBContext context)
        {
            Console.WriteLine("Applying migration");
            context.Database.Migrate();
            if (!context.Products.Any())
            {
                context.Products.AddRange(
                    new Product { Name = "Television", Price = 700, Quantity = 10 },
                    new Product { Name = "Shirt", Price = 100, Quantity = 20 },
                    new Product { Name = "IPhone 11", Price = 1000, Quantity = 30}
                    );
                
                context.SaveChanges();
            }
        }
    }
}
