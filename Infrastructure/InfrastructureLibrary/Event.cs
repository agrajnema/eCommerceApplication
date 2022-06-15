using System;
using System.Collections.Generic;
using System.Text;

namespace InfrastructureLibrary
{
    public class Event : Message
    {
        public Event()
        {
        }

        public Event(Guid messageId) : base(messageId)
        {
        }

        public Event(string messageType, DateTime createdDate) : base(messageType, createdDate)
        {
        }

        public Event(Guid messageId, string messageType, DateTime createdDate) : base(messageId, messageType, createdDate)
        {
        }
    }
}
