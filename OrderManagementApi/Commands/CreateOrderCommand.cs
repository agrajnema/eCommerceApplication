using InfrastructureLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderManagementApi.Commands
{
    public class CreateOrderCommand : Command
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public int ProductId { get; set; }
        public DateTime OrderDate { get; set; }

        public CreateOrderCommand(Guid messageId, int orderId, int customerId, int productId, DateTime orderDate): base(messageId)
        {
            OrderId = orderId;
            CustomerId = customerId;
            ProductId = productId;
            OrderDate = orderDate;
        }
    }
}
