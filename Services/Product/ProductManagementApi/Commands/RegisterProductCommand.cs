using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InfrastructureLibrary;

namespace ProductManagementApi.Commands
{
    public class RegisterProductCommand : Command
    {
        public readonly string Id;
        public readonly string Name;
        public readonly string Description;
        public readonly int Quantity;
        public readonly decimal Price;
        public readonly string Summary;
        public readonly string Category;

        public RegisterProductCommand(Guid messageId, string id, string name, string description, int quantity, decimal price, string summary, string category) : base(messageId)
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
