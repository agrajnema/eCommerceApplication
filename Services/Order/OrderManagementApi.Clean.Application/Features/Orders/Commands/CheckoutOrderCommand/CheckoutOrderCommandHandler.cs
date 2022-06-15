using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using OrderManagementApi.Clean.Application.Contracts.Infrastucture;
using OrderManagementApi.Clean.Application.Contracts.Persistence;
using OrderManagementApi.Clean.Application.Models;
using OrderManagementApi.Clean.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OrderManagementApi.Clean.Application.Features.Orders.Commands.CheckoutOrderCommand
{
    public class CheckoutOrderCommandHandler : IRequestHandler<CheckoutOrderCommand, int>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly ILogger<CheckoutOrderCommandHandler> _logger;

        public CheckoutOrderCommandHandler(IOrderRepository orderRepository, IMapper mapper, IEmailService emailService, ILogger<CheckoutOrderCommandHandler> logger)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _emailService = emailService;
            _logger = logger;
        }
        public async Task<int> Handle(CheckoutOrderCommand request, CancellationToken cancellationToken)
        {
            var orderEntity = _mapper.Map<Order>(request);
            var newOrder = await _orderRepository.AddAsync(orderEntity);
            if (newOrder?.Id != null)
            {
                await SendEmail(newOrder);
            }
            return newOrder.Id;

        }

        private async Task SendEmail(Order order)
        {
            var email = new Email { To = "agrajnema@gmail.com", Body = $"Order with Order ID: {order.Id} with Total: {order.TotalPrice} created successfully", Subject = $"Order {order.Id} created successfully" };
            try
            {
                await _emailService.SendEmail(email);
            }
            catch(Exception ex)
            {
                _logger.LogError($"Order {order.Id} failed due to an error with the mail service: {ex.Message}");

            }
        }
    }
}
