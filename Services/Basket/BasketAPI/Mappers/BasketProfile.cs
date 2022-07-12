using AutoMapper;
using BasketAPI.Entities;
using InfrastructureLibrary.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BasketAPI.Mappers
{
    public class BasketProfile : Profile
    {
        public BasketProfile()
        {
            CreateMap<BasketCheckoutEvent, BasketCheckout>().ReverseMap();
        }
    }
}
