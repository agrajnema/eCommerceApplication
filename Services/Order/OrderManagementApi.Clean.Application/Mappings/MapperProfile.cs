using AutoMapper;
using OrderManagementApi.Clean.Application.Features.Orders.Commands.CheckoutOrderCommand;
using OrderManagementApi.Clean.Application.Features.Orders.Queries.GetOrderList;
using OrderManagementApi.Clean.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementApi.Clean.Application.Mappings
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Order, OrderVm>().ReverseMap();
            CreateMap<Order, CheckoutOrderCommand>().ReverseMap();
        }
    }
}
