﻿using CustomerManagementApi.Commands;
using CustomerManagementApi.Events;
using CustomerManagementApi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerManagementApi.Mapper
{
    public static class CustomerMapper
    {
        public static Customer MapCustomerCommandToCustomer(this RegisterCustomerCommand registerCustomerCommand) => new Customer
        {
            CustomerId = registerCustomerCommand.CustomerId,
            FirstName = registerCustomerCommand.FirstName,
            LastName = registerCustomerCommand.LastName,
            EmailAddress = registerCustomerCommand.EmailAddress
        };

        public static CustomerRegisteredEvent MapCustomerCommandToEvent(this RegisterCustomerCommand command) => new CustomerRegisteredEvent
        (
          Guid.NewGuid(),
          command.FirstName,
          command.LastName,
          command.EmailAddress 
        );
    }
}
