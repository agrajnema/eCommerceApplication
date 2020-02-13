using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerceWebApp.Models.Product
{
    public class Product
    {
        public int ProductId { get; set; }
        [Required]
        [Display(Name="Name")]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        [Display(Name = "Quantity")]
        public int Quantity { get; set; }
        [Required]
        [Display(Name = "Price")]
        public decimal Price { get; set; }
    }
}
