using OrderManagementApi.Commands;
using OrderManagementApi.Events;
using OrderManagementApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderManagementApi.Mapper
{
    public static class OrderMapper
    {
        public static Order MapOrderCommandToOrder(this CreateOrderCommand createOrderCommand) => new Order
        {
            OrderId = createOrderCommand.OrderId,
            CustomerId = createOrderCommand.CustomerId,
            ProductId = createOrderCommand.ProductId,
            OrderDate = createOrderCommand.OrderDate
        };

        public static OrderCreatedEvent MapOrderCommandToEvent(this CreateOrderCommand createOrderCommand) => new OrderCreatedEvent
        (
            Guid.NewGuid(),
            createOrderCommand.OrderId,
            createOrderCommand.CustomerId,
            createOrderCommand.ProductId,
            createOrderCommand.OrderDate
        );
    }
}
