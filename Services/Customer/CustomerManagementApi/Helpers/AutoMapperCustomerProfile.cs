using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CustomerManagementApi.Commands;
using CustomerManagementApi.Model;

namespace CustomerManagementApi.Helpers
{
    public class AutoMapperCustomerProfile : Profile
    {
        public AutoMapperCustomerProfile()
        {
            CreateMap<Customer, CustomerModel>();
            CreateMap<RegisterCustomerModel, Customer>();
            CreateMap<RegisterCustomerCommand, Customer>();
        }
    }
}
