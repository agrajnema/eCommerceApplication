using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InfrastructureLibrary;

namespace ProductManagementApi.Commands
{
    public class RegisterProductCommand : Command
    {
        public readonly int ProductId;
        public readonly string Name;
        public readonly string Description;
        public readonly int Quantity;
        public readonly decimal Price;

        public RegisterProductCommand(Guid messageId, int productId, string name, string description, int quantity, decimal price) : base(messageId)
        {
            ProductId = productId;
            Name = name;
            Description = description;
            Quantity = quantity;
            Price = price;
        }
    }
}
