using eCommerceWebApp.Commands;
using eCommerceWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerceWebApp.Mappers
{
    public static class ProductMapper
    {
        public static RegisterProductCommand MapToRegisterProduct(this Product product)
        {
            return new RegisterProductCommand(
            Guid.NewGuid(),
            product.ProductId,
            product.Name,
            product.Description,
            product.Quantity,
            product.Price);
        }
    }
}
