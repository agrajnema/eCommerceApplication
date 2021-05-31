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

namespace ProductManagementApi.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly IMessagePublisher _rabbitMQPublisher;

        public ProductController(IProductRepository productRepository, IMessagePublisher rabbitMQPublisher)
        {
            _productRepository = productRepository;
            _rabbitMQPublisher = rabbitMQPublisher;
        }

        [HttpGet]
        [Route("product")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _productRepository.GetProducts());
        }

        [HttpGet]
        [Route("product/{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _productRepository.GetProductById(id);
            if (product == null)
                return NotFound();
            return Ok(product);
        }

        [HttpPost]
        [Route("product")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterProductCommand productCommand)
        {
                //save to db
                var product = productCommand.MapProductCommandToProduct();
                await _productRepository.RegisterProduct(product);
                //publish event to RabbitMQ
                var productEvent = productCommand.MapProductCommandToProductRegisteredEvent();
                await _rabbitMQPublisher.PublishMessageAsync(productEvent.MessageType, productEvent, "");
                return RedirectToAction("GetByProductId", new { id = product.ProductId});
        } 
    }
}