using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using eCommerceWebApp.Models;
using eCommerceWebApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace eCommerceWebApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IProductService _productService;
        private readonly IBasketService _basketService;

        public IndexModel(IProductService productService, IBasketService basketService)
        {
            _productService = productService ?? throw new ArgumentNullException(nameof(productService));
            _basketService = basketService ?? throw new ArgumentNullException(nameof(basketService));
        }

        public IEnumerable<Models.ProductModel> ProductList { get; set; } = new List<Models.ProductModel>();

        public async Task<IActionResult> OnGetAsync()
        {
            ProductList = await _productService.GetProduct();
            return Page();
        }

        public async Task<IActionResult> OnPostAddToCartAsync(string productId)
        {
            var product = await _productService.GetProduct(productId);

            var userName = "swn";
            var basket = await _basketService.GetBasket(userName);

            basket.Items.Add(new BasketItemModel
            {
                ProductId = productId,
                ProductName = product.Name,
                Price = product.Price,
                Quantity = 1,
                Color = "Black"
            });

            var basketUpdated = await _basketService.UpdateBasket(basket);
            return RedirectToPage("Cart");
        }
    }
}
