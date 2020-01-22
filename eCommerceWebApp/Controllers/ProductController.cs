using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using eCommerceWebApp.Mappers;
using eCommerceWebApp.Models;
using eCommerceWebApp.RestClients;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace eCommerceWebApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductManagementApiClient _productManagementApiClient;

        public ProductController(IProductManagementApiClient productManagementApiClient)
        {
            _productManagementApiClient = productManagementApiClient;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var model = await _productManagementApiClient.GetProducts();
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var model = await _productManagementApiClient.GetProductById(id);
            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new Product();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromForm] Product product)
        {
            if (ModelState.IsValid)
            {
                var productRegisterCommand = product.MapToRegisterProduct();
                await _productManagementApiClient.RegisterProduct(productRegisterCommand);
                return RedirectToAction("Index");
            }
            else
            {
                return View("New", product);
            }
        }
    }
}