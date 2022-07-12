using Shopping.Aggregator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shopping.Aggregator.Services.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderModel>> GetOrdersByUserName(string userName);
    }
}
