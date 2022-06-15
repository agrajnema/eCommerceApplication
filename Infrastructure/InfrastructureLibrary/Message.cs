using System;

namespace InfrastructureLibrary
{
    public class Message
    {
        public readonly Guid MessageId;
        public readonly string MessageType;
        public readonly DateTime CreatedDate;

        public Message() : this(Guid.NewGuid())
        {
        }

        public Message(Guid messageId)
        {
            MessageId = messageId;
            MessageType = this.GetType().Name;
            CreatedDate = DateTime.Now;
        }

        public Message(string messageType, DateTime createdDate) : this(Guid.NewGuid())
        {
            MessageType = messageType;
            CreatedDate = createdDate;
        }

        public Message(Guid messageId, string messageType, DateTime createdData)
        {
            MessageId = messageId;
            MessageType = messageType;
            CreatedDate = createdData;
        }
    }
}
