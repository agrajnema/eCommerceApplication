using System;
using System.Collections.Generic;
using System.Text;

namespace InfrastructureLibrary
{
    public interface IMessageHandler
    {
        void Start(IMessageHandlerCallback callback);
        void Stop();
    }
}
