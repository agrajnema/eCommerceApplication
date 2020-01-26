using CustomerManagementApi.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerManagementApi.DataAccess
{
    public class CustomerDBContext: DbContext
    {
        public CustomerDBContext(DbContextOptions<CustomerDBContext> options): base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Customer>().HasKey(c => c.CustomerId);
            builder.Entity<Customer>().ToTable("Customers");
            base.OnModelCreating(builder);
        }
        public DbSet<Customer> Customers { get; set; }
    }
}
