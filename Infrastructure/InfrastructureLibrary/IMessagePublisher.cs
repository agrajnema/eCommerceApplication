using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureLibrary
{
    public interface IMessagePublisher
    {
        //message type - type of message
        //message - message to publish
        //routing key - key used by publisher
        Task PublishMessageAsync(string messageType, object message, string routingKey);
    }
}
