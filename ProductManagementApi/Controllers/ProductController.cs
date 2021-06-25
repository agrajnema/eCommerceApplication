using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductManagementApi.Commands;
using ProductManagementApi.Models;
using ProductManagementApi.Repository;
using ProductManagementApi.Mappers;
using Newtonsoft.Json;
using InfrastructureLibrary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System.Net;

namespace ProductManagementApi.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductMongoRepository _productRepository;
       // private readonly IMessagePublisher _rabbitMQPublisher;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IProductMongoRepository productRepository, ILogger<ProductController> logger /*, IMessagePublisher rabbitMQPublisher*/)
        {
            _productRepository = productRepository;
          //  _rabbitMQPublisher = rabbitMQPublisher;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var products = await _productRepository.GetProducts();
            return Ok(products);
        }

        [HttpGet("{id:length(24)}", Name = "GetProduct")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Product>> GetProductById(string id)
        {
            var product = await _productRepository.GetProduct(id);
            if (product == null)
            {
                _logger.LogError($"Product with id: {id}, not found.");
                return NotFound();
            }
            return Ok(product);
        }

        [Route("[action]/{category}", Name = "GetProductByCategory")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductByCategory(string category)
        {
            var products = await _productRepository.GetProductByCategory(category);
            return Ok(products);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Product>> CreateProduct([FromBody] RegisterProductCommand productCommand)
        {
            var product = productCommand.MapProductCommandToProduct();
            await _productRepository.CreateProduct(product);
            //publish event to RabbitMQ
            var productEvent = productCommand.MapProductCommandToProductRegisteredEvent();
          //  await _rabbitMQPublisher.PublishMessageAsync(productEvent.MessageType, productEvent, "");
            return CreatedAtRoute("GetProduct", new { id = product.Id }, product);
        }

        [HttpPut]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateProduct([FromBody] Product product)
        {
            return Ok(await _productRepository.UpdateProduct(product));
        }

        [HttpDelete("{id:length(24)}", Name = "DeleteProduct")]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteProductById(string id)
        {
            return Ok(await _productRepository.DeleteProduct(id));
        }

        //[HttpPost]
        //[Route("product")]
        //public async Task<IActionResult> RegisterAsync([FromBody] RegisterProductCommand productCommand)
        //{
        //        //save to db
        //        var product = productCommand.MapProductCommandToProduct();
        //        await _productRepository.RegisterProduct(product);
        //        //publish event to RabbitMQ
        //        var productEvent = productCommand.MapProductCommandToProductRegisteredEvent();
        //        await _rabbitMQPublisher.PublishMessageAsync(productEvent.MessageType, productEvent, "");
        //        return RedirectToAction("GetByProductId", new { id = product.ProductId});
        //} 
    }
}