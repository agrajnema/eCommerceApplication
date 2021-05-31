using CustomerManagementApi.DataAccess;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerManagementApi.Model
{
    public static class DBSeeder
    {
        public static void PopulateDB(IApplicationBuilder builder)
        {
            using(var serviceScope = builder.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<CustomerDBContext>());
            }
        }

        private static void SeedData(CustomerDBContext customerDBContext)
        {
            Console.WriteLine("Applying Customer Migration");
            customerDBContext.Database.Migrate();
            Console.WriteLine("Migration applied successfully");
        }
    }
}
