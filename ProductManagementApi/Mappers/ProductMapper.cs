using ProductManagementApi.Commands;
using ProductManagementApi.Events;
using ProductManagementApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductManagementApi.Mappers
{
    public static class ProductMapper
    {
        public static Product MapProductCommandToProduct(this RegisterProductCommand command) => new Product
        {
            ProductId = command.ProductId,
            Name = command.Name,
            Description = command.Description,
            Price = command.Price,
            Quantity = command.Quantity
        };

        public static ProductRegisteredEvent MapProductCommandToProductRegisteredEvent(this RegisterProductCommand command) => new ProductRegisteredEvent
        (
            Guid.NewGuid(),
            command.ProductId,
            command.Name,
            command.Description,
            command.Quantity,
            command.Price
        );

    }
}
