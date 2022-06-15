using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InfrastructureLibrary;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderManagementApi.Commands;
using OrderManagementApi.Mapper;
using OrderManagementApi.Repository;

namespace OrderManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMessagePublisher _rabbitMQPublisher;

        public OrderController(IOrderRepository orderRepository, IMessagePublisher rabbitMQPublisher)
        {
            _orderRepository = orderRepository;
            _rabbitMQPublisher = rabbitMQPublisher;
        }

        [Route("order/{customerId}")]
        public async Task<IActionResult> GetAllOrdersByCustomerId(int customerId)
        {
            return Ok(await _orderRepository.GetAllOrdersByCustomerId(customerId));
        }

        [HttpPost]
        [Route("order")]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderCommand createOrderCommand)
        {
            //save to db
            var order = createOrderCommand.MapOrderCommandToOrder();
            await _orderRepository.CreateOrder(order);

            //publish to rabbitMQ
            var createOrderEvent = createOrderCommand.MapOrderCommandToEvent();
            return Ok();
        }
    }
}