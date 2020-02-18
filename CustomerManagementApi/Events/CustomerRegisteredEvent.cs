using CustomerManagementApi.Commands;
using InfrastructureLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerManagementApi.Events
{
    public class CustomerRegisteredEvent: Event
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }

        public CustomerRegisteredEvent(Guid messageId, string firstName, string lastName, string emailAddress): base(messageId)  
        {
            FirstName = firstName;
            LastName = lastName;
            EmailAddress = emailAddress;
        }
        
    }
}
