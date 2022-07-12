using Microsoft.EntityFrameworkCore;
using OrderManagementApi.DataAccess;
using OrderManagementApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderManagementApi.Repository
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAllOrdersByCustomerId(int customerId);
        Task CreateOrder(Order order);
    }
    public class OrderRepository : IOrderRepository
    {
        private readonly OrderDbContext _context;

        public OrderRepository(OrderDbContext context)
        {
            _context = context;
        }
        public async Task CreateOrder(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Order>> GetAllOrdersByCustomerId(int customerId)
        {
            if (!_context.Orders.Any())
                return null;

            return await _context.Orders.Where(o => o.CustomerId == customerId).ToListAsync();
        }
    }
}
