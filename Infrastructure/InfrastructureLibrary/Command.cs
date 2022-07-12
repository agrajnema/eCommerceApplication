using System;
using System.Collections.Generic;
using System.Text;

namespace InfrastructureLibrary
{
    public class Command : Message
    {
        public Command()
        {
        }

        public Command(Guid messageId) : base(messageId)
        {
        }

        public Command(string messageType, DateTime createdDate) : base(messageType, createdDate)
        {
        }

        public Command(Guid messageId, string messageType, DateTime createdDate) : base(messageId, messageType, createdDate)
        {
        }
    }
}
