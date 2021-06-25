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
            Id = command.Id,
            Name = command.Name,
            Description = command.Description,
            Price = command.Price,
            Quantity = command.Quantity,
            Summary = command.Summary,
            Category = command.Category
        };

        public static ProductRegisteredEvent MapProductCommandToProductRegisteredEvent(this RegisterProductCommand command) => new ProductRegisteredEvent
        (
            Guid.NewGuid(),
            command.Id,
            command.Name,
            command.Description,
            command.Quantity,
            command.Price,
            command.Summary,
            command.Category
        );

    }
}
