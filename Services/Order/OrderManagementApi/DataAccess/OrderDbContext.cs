using Microsoft.EntityFrameworkCore;
using OrderManagementApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderManagementApi.DataAccess
{
    public class OrderDbContext : DbContext
    {
        public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Order>().HasKey(o => o.OrderId);
            builder.Entity<Order>().ToTable("Orders");
            base.OnModelCreating(builder);
        }

        public DbSet<Order> Orders { get; set; }
    }
}
