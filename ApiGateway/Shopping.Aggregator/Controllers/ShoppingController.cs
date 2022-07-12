using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Shopping.Aggregator.Models;
using Shopping.Aggregator.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Shopping.Aggregator.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ShoppingController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IBasketService _basketService;
        private readonly IOrderService _orderService;

        public ShoppingController(IProductService productService, IBasketService basketService, IOrderService orderService)
        {
            _productService = productService ?? throw new ArgumentNullException(nameof(productService));
            _basketService = basketService ?? throw new ArgumentNullException(nameof(basketService));
            _orderService = orderService ?? throw new ArgumentNullException(nameof(orderService));
        }

        [HttpGet("{userName}", Name = "GetShopping")]
        [ProducesResponseType(typeof(ShoppingModel), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetShopping(string userName)
        {
            var basket = await _basketService.GetBasket(userName);

            foreach(var basketItem in basket.Items)
            {
                var product = await _productService.GetProduct(basketItem.ProductId);
                // set additional product fields
                basketItem.ProductName = product.Name;
                basketItem.Category = product.Category;
                basketItem.Summary = product.Summary;
                basketItem.Description = product.Description;
                basketItem.ImageFile = product.ImageFile;
            }

            var orders = await _orderService.GetOrdersByUserName(userName);

            var shoppingModel = new ShoppingModel
            {
                UserName = userName,
                BasketWithProducts = basket,
                Orders = orders
            };
            return Ok(shoppingModel);
        }
    }
}
