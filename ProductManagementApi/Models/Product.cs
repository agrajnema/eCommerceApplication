using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductManagementApi.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string Id { get; internal set; }
        public string Summary { get; internal set; }
        public string ImageFile { get; internal set; }
        public string Category { get; internal set; }
    }
}
