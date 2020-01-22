using InfrastructureLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductManagementApi.Events
{
    public class ProductRegisteredEvent : Event
    {
        public readonly int ProductId;
        public readonly string Name;
        public readonly string Description;
        public readonly int Quantity;
        public readonly decimal Price;

        public ProductRegisteredEvent(Guid messageId, int productId, string name, string description, int quantity, decimal price) : base(messageId)
        {
            ProductId = productId;
            Name = name;
            Description = description;
            Quantity = quantity;
            Price = price;
        }
    }
}
