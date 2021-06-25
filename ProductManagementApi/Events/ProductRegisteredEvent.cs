using InfrastructureLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductManagementApi.Events
{
    public class ProductRegisteredEvent : Event
    {
        public readonly string Id;
        public readonly string Name;
        public readonly string Description;
        public readonly int Quantity;
        public readonly decimal Price;
        public readonly string Summary;
        public readonly string Category;

        public ProductRegisteredEvent(Guid messageId, string id, string name, string description, int quantity, decimal price, string summary, string category) : base(messageId)
        {
            Id = id;
            Name = name;
            Description = description;
            Quantity = quantity;
            Price = price;
            Summary = summary;
            Category = category;
        }
    }
}
