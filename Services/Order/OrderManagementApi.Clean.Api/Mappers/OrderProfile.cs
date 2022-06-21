using AutoMapper;
using InfrastructureLibrary.Events;
using OrderManagementApi.Clean.Application.Features.Orders.Commands.CheckoutOrderCommand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderManagementApi.Clean.Api.Mappers
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<CheckoutOrderCommand, BasketCheckoutEvent>().ReverseMap();
        }
    }
}
