using AutoMapper;
using InfrastructureLibrary.Events;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using OrderManagementApi.Clean.Application.Features.Orders.Commands.CheckoutOrderCommand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderManagementApi.Clean.Api.EventBusConsumer
{
    public class BasketCheckoutConsumer : IConsumer<BasketCheckoutEvent>
    {
        private readonly ILogger<BasketCheckoutConsumer> _logger;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public BasketCheckoutConsumer(ILogger<BasketCheckoutConsumer> logger, IMediator mediator, IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
        {
            var command = _mapper.Map<CheckoutOrderCommand>(context.Message);
            var result = await _mediator.Send(command);

            _logger.LogInformation("BasketCheckoutEvent consumed successfully. Created Order Id : {newOrderId}", result);
        }
    }
}
