using InfrastructureLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerceWebApp.Commands
{
    public class RegisterCustomerCommand : Command
    {
        public int CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }

        public RegisterCustomerCommand(Guid messageId, int customerId, string firstName, string lastName, string emailAddress, string password): base(messageId)
        {
            CustomerId = customerId;
            FirstName = firstName;
            LastName = lastName;
            EmailAddress = emailAddress;
            Password = password;
        }
    }
}
