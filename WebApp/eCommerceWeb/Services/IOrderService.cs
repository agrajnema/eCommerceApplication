using eCommerceWebApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eCommerceWebApp.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderResponseModel>> GetOrdersByUserName(string userName);
    }

}
